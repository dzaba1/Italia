using Dzaba.Utils;
using Italia.Lib.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Italia.Lib.Dal
{
    public interface IOffersDal
    {
        Task<Offer[]> GetAllActiveAsync();
    }

    internal sealed class OffersDal : IOffersDal
    {
        private readonly DatabaseContext dbContext;

        public OffersDal(DatabaseContext dbContext)
        {
            Require.NotNull(dbContext, nameof(dbContext));

            this.dbContext = dbContext;
        }

        public async Task<Offer[]> GetAllActiveAsync()
        {
            return await dbContext.Offers
                .Where(o => o.Active)
                .ToArrayAsync();
        }
    }
}
