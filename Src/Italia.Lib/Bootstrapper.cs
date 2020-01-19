using Italia.Lib.Dal;
using Italia.Lib.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace Italia.Lib
{
    public static class Bootstrapper
    {
        public static void RegisterItalia(this IServiceCollection container)
        {
            container.AddTransient<IItaliaEngine, ItaliaEngine>();
            container.AddTransient<IOffersDal, OffersDal>();
            container.AddTransient<ISettings, Settings>();
            container.AddTransient<IDataProviderComposite, DataProviderComposite>();
        }
    }
}
