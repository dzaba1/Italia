using Dzaba.Utils;
using Italia.Lib.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Italia.Lib
{
    public interface IDataProviderComposite
    {
        Task<IReadOnlyDictionary<ReferenceKey, Offer>> PollOffersAsync();
    }

    internal sealed class DataProviderComposite : IDataProviderComposite
    {
        private readonly IDataProvider[] dataProviders;

        public DataProviderComposite(IEnumerable<IDataProvider> dataProviders)
        {
            Require.NotNull(dataProviders, nameof(dataProviders));

            this.dataProviders = dataProviders.ToArray();
        }

        public async Task<IReadOnlyDictionary<ReferenceKey, Offer>> PollOffersAsync()
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
