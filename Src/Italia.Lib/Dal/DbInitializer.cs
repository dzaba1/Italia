using System;
using System.Threading.Tasks;
using Dzaba.Utils;
using Microsoft.Extensions.Logging;

namespace Italia.Lib.Dal
{
    public interface IDbInitializer
    {
        Task InitAsync();
    }

    internal sealed class DbInitializer : IDbInitializer
    {
        private readonly Func<DatabaseContext> dbContextFactory;
        private readonly ILogger<DbInitializer> logger;

        public DbInitializer(Func<DatabaseContext> dbContextFactory,
            ILogger<DbInitializer> logger)
        {
            Require.NotNull(dbContextFactory, nameof(dbContextFactory));
            Require.NotNull(logger, nameof(logger));

            this.dbContextFactory = dbContextFactory;
            this.logger = logger;
        }

        public async Task InitAsync()
        {
            logger.LogDebug("Initializing the DB.");

            using (var dbContext = dbContextFactory())
            {
                var created = await dbContext.Database.EnsureCreatedAsync();

                if (created)
                {
                    logger.LogInformation("Created the DB");
                }
            }
        }
    }
}
