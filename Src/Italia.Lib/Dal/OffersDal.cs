using Dzaba.Utils;
using Italia.Lib.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Italia.Lib.Dal
{
    public interface IOffersDal
    {
        Task<Offer[]> GetAllAsync();
        Task UpdateAsync(Offer value);
        Task<int> AddAsync(Offer value);
    }

    internal sealed class OffersDal : IOffersDal
    {
        private readonly DatabaseContext dbContext;

        public OffersDal(DatabaseContext dbContext)
        {
            Require.NotNull(dbContext, nameof(dbContext));

            this.dbContext = dbContext;
        }

        public async Task<int> AddAsync(Offer value)
        {
            Require.NotNull(value, nameof(value));

            dbContext.Offers.Add(value);
            await dbContext.SaveChangesAsync();
            return value.Id;
        }

        public async Task<Offer[]> GetAllAsync()
        {
            return await dbContext.Offers
                .ToArrayAsync();
        }

        public async Task UpdateAsync(Offer value)
        {
            Require.NotNull(value, nameof(value));

            dbContext.Offers.Update(value);
            await dbContext.SaveChangesAsync();
        }
    }
}
