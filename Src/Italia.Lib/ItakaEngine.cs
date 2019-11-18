using Dzaba.Utils;
using Italia.Lib.Dal;
using Italia.Lib.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Italia.Lib
{
    public interface IItaliaEngine
    {

    }

    internal sealed class ItaliaEngine : IItaliaEngine
    {
        private readonly IDataProvider[] dataProviders;
        private readonly IOffersDal offersDal;

        public ItaliaEngine(IEnumerable<IDataProvider> dataProviders,
            IOffersDal offersDal)
        {
            Require.NotNull(dataProviders, nameof(dataProviders));
            Require.NotNull(offersDal, nameof(offersDal));

            this.dataProviders = dataProviders.ToArray();
            this.offersDal = offersDal;
        }

        public async Task RunAsync()
        {
            var savedOffers = await offersDal.GetAllAsync()
                .ConfigureAwait(false);
            var polled = await PollOffers()
                .ConfigureAwait(false);

            
        }

        private async Task<IReadOnlyDictionary<ReferenceKey, Offer>> PollOffers()
        {
            var offers = new Dictionary<ReferenceKey, Offer>();
            var tasks = new List<Task<Offer[]>>(dataProviders.Length);

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
    }
}
