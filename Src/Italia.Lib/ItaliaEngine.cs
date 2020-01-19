using Dzaba.Utils;
using Italia.Lib.Dal;
using Italia.Lib.Model;
using Italia.Lib.Notifications;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Italia.Lib
{
    public interface IItaliaEngine
    {
        Task RunAsync();
    }

    internal sealed class ItaliaEngine : IItaliaEngine
    {
        private readonly IDataProviderComposite dataProviders;
        private readonly IOffersDal offersDal;

        public ItaliaEngine(IDataProviderComposite dataProviders,
            IOffersDal offersDal)
        {
            Require.NotNull(dataProviders, nameof(dataProviders));
            Require.NotNull(offersDal, nameof(offersDal));

            this.dataProviders = dataProviders;
            this.offersDal = offersDal;
        }

        public async Task RunAsync()
        {
            var toNotify = await ProcessAsync();
        }

        public async Task<OffersToNotify> ProcessAsync()
        {
            var savedOffers = (await offersDal.GetAllAsync()
                .ConfigureAwait(false))
                .ToDictionary(o => o.GetReferenceKey());
            var polled = await dataProviders.PollOffersAsync()
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

        private async Task OfferIsGoneAsync(Offer offer, OffersToNotify offersToNotify)
        {
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
            SetProperties(offer, savedOffer);
            offer.Active = true;

            await offersDal.UpdateAsync(offer);
            offersToNotify.AddOfferActiveAgain(offer);
        }

        private async Task OfferChangedAsync(Offer offer, Offer savedOffer, OffersToNotify offersToNotify)
        {
            SetProperties(offer, savedOffer);

            await offersDal.UpdateAsync(offer);
            offersToNotify.AddOfferChanged(savedOffer, offer);
        }

        private async Task NewOfferAsync(Offer offer, OffersToNotify offersToNotify)
        {
            offer.Active = true;

            var now = DateTime.Now;
            offer.Modified = now;
            offer.Created = now;

            await offersDal.AddAsync(offer);
            offersToNotify.AddNewOffer(offer);
        }
    }
}
