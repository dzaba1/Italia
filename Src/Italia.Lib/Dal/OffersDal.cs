using System;
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
        private readonly Func<DatabaseContext> dbContextFactory;

        public OffersDal(Func<DatabaseContext> dbContextFactory)
        {
            Require.NotNull(dbContextFactory, nameof(dbContextFactory));

            this.dbContextFactory = dbContextFactory;
        }

        public async Task<int> AddAsync(Offer value)
        {
            Require.NotNull(value, nameof(value));

            using (var dbContext = dbContextFactory())
            {
                dbContext.Offers.Add(value);
                await dbContext.SaveChangesAsync();
                return value.Id;
            }
        }

        public async Task<Offer[]> GetAllAsync()
        {
            using (var dbContext = dbContextFactory())
            {
                return await dbContext.Offers
                    .ToArrayAsync();
            }
        }

        public async Task UpdateAsync(Offer value)
        {
            Require.NotNull(value, nameof(value));

            using (var dbContext = dbContextFactory())
            {
                dbContext.Offers.Update(value);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
