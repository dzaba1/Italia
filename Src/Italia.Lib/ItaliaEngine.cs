using Dzaba.Utils;
using Italia.Lib.Dal;
using Italia.Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Italia.Lib.Notifications;
using Microsoft.Extensions.Logging;

namespace Italia.Lib
{
    public interface IItaliaEngine
    {
        Task RunAsync();
    }

    internal sealed class ItaliaEngine : IItaliaEngine
    {
        private readonly IOffersDal offersDal;
        private readonly INotificationsManager notificationsManager;
        private readonly IDataProvider[] dataProviders;
        private readonly ILogger<ItaliaEngine> logger;

        public ItaliaEngine(IEnumerable<IDataProvider> dataProviders,
            IOffersDal offersDal,
            INotificationsManager notificationsManager,
            ILogger<ItaliaEngine> logger)
        {
            Require.NotNull(dataProviders, nameof(dataProviders));
            Require.NotNull(offersDal, nameof(offersDal));
            Require.NotNull(notificationsManager, nameof(notificationsManager));
            Require.NotNull(logger, nameof(logger));

            this.dataProviders = dataProviders.ToArray();
            this.offersDal = offersDal;
            this.notificationsManager = notificationsManager;
            this.logger = logger;
        }

        public async Task RunAsync()
        {
            var toNotify = await ProcessAsync();
            await notificationsManager.NotifyAsync(toNotify);
        }

        public async Task<OffersToNotify> ProcessAsync()
        {
            var savedOffers = (await offersDal.GetAllAsync()
                .ConfigureAwait(false))
                .ToDictionary(o => o.GetReferenceKey());
            var polled = await PollOffersAsync()
                .ConfigureAwait(false);
            var offersToNotify = new OffersToNotify();

            foreach (var offer in polled)
            {
                if (savedOffers.TryGetValue(offer.Key, out var savedOffer))
                {
                    await OfferExistsAsync(offersToNotify, offer.Value, savedOffer);
                }
                else
                {
                    await NewOfferAsync(offer.Value, offersToNotify);
                }

                savedOffers.Remove(offer.Key);
            }

            var goneOffers = savedOffers.Values
                .Where(o => o.Active);

            foreach (var offer in goneOffers)
            {
                await OfferIsGoneAsync(offer, offersToNotify);
            }

            return offersToNotify;
        }

        private async Task<IReadOnlyDictionary<ReferenceKey, Offer>> PollOffersAsync()
        {
            var offers = new Dictionary<ReferenceKey, Offer>();
            var tasks = new List<Task<Offer[]>>(dataProviders.Length);

            logger.LogInformation("Getting all data from providers.");

            foreach (var dataProvider in dataProviders)
            {
                var task = dataProvider.GetOffersAsync();
                tasks.Add(task);
            }

            foreach (var task in tasks)
            {
                var polled = await task;
                foreach (var offer in polled)
                {
                    offers.Add(offer.GetReferenceKey(), offer);
                }
            }

            return offers;
        }

        private async Task OfferIsGoneAsync(Offer offer, OffersToNotify offersToNotify)
        {
            logger.LogInformation($"Offer with ID {offer.Id} is not active.");

            offer.Active = false;
            offer.Modified = DateTime.Now;

            await offersDal.UpdateAsync(offer);
            offersToNotify.AddOfferIsGone(offer);
        }

        private async Task OfferExistsAsync(OffersToNotify offersToNotify, Offer offer, Offer savedOffer)
        {
            if (savedOffer.Active)
            {
                if (!OfferChangedComparer.Instance.Equals(offer, savedOffer))
                {
                    await OfferChangedAsync(offer, savedOffer, offersToNotify);
                }
            }
            else
            {
                await OfferActiveAgain(offersToNotify, offer, savedOffer);
            }
        }

        private void SetProperties(Offer offer, Offer savedOffer)
        {
            offer.Id = savedOffer.Id;
            offer.Created = savedOffer.Created;
            offer.Modified = DateTime.Now;
        }

        private async Task OfferActiveAgain(OffersToNotify offersToNotify, Offer offer, Offer savedOffer)
        {
            logger.LogInformation($"Offer with ID {offer.Id} active again.");

            SetProperties(offer, savedOffer);
            offer.Active = true;

            await offersDal.UpdateAsync(offer);
            offersToNotify.AddOfferActiveAgain(savedOffer, offer);
        }

        private async Task OfferChangedAsync(Offer offer, Offer savedOffer, OffersToNotify offersToNotify)
        {
            logger.LogInformation($"Offer with ID {offer.Id} changed.");

            SetProperties(offer, savedOffer);

            await offersDal.UpdateAsync(offer);
            offersToNotify.AddOfferChanged(savedOffer, offer);
        }

        private async Task NewOfferAsync(Offer offer, OffersToNotify offersToNotify)
        {
            logger.LogInformation($"New offer from '{offer.DataProvider}' - {offer.ExternalReference}");

            offer.Active = true;

            var now = DateTime.Now;
            offer.Modified = now;
            offer.Created = now;

            await offersDal.AddAsync(offer);
            offersToNotify.AddNewOffer(offer);
        }
    }
}
