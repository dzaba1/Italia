using System;
using System.Threading.Tasks;
using Dzaba.Utils;
using Italia.Lib;
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
        private readonly IItaliaEngine italiaEngine;

        public App(IDbInitializer dbInitializer,
            IItaliaEngine italiaEngine)
        {
            Require.NotNull(dbInitializer, nameof(dbInitializer));
            Require.NotNull(italiaEngine, nameof(italiaEngine));

            this.dbInitializer = dbInitializer;
            this.italiaEngine = italiaEngine;
        }

        public void Dispose()
        {
            
        }

        public async Task RunAsync()
        {
            await dbInitializer.InitAsync();
            await italiaEngine.RunAsync();
        }
    }
}
