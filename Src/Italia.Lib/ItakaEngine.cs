using Dzaba.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Italia.Lib
{
    public interface IItaliaEngine
    {

    }

    internal sealed class ItaliaEngine : IItaliaEngine
    {
        private readonly IDataProvider[] dataProviders;

        public ItaliaEngine(IEnumerable<IDataProvider> dataProviders)
        {
            Require.NotNull(dataProviders, nameof(dataProviders));

            this.dataProviders = dataProviders.ToArray();
        }
    }
}
