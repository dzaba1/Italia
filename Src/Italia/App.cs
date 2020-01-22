using System;
using System.Threading.Tasks;
using Dzaba.Utils;
using Italia.Lib.Dal;

namespace Italia
{
    internal interface IApp : IDisposable
    {
        Task RunAsync();
    }

    internal sealed class App : IApp
    {
        private readonly IDbInitializer dbInitializer;

        public App(IDbInitializer dbInitializer)
        {
            Require.NotNull(dbInitializer, nameof(dbInitializer));

            this.dbInitializer = dbInitializer;
        }

        public void Dispose()
        {
            
        }

        public async Task RunAsync()
        {
            await dbInitializer.InitAsync();
        }
    }
}
