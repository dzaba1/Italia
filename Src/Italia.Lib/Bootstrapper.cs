using Dzaba.Utils;
using Italia.Lib.Dal;
using Italia.Lib.Notifications;
using Italia.Lib.Notifications.Email;
using Microsoft.Extensions.DependencyInjection;

namespace Italia.Lib
{
    public static class Bootstrapper
    {
        public static void RegisterItalia(this IServiceCollection container)
        {
            Require.NotNull(container, nameof(container));

            container.AddTransient<IItaliaEngine, ItaliaEngine>();
            container.AddTransient<IOffersDal, OffersDal>();
            container.AddTransient<INotificationsManager, NotificationsManager>();
            container.AddTransient<INotification, EmailNotification>();
            container.AddTransient<IEmailBodyBuilder, EmailBodyBuilder>();
        }
    }
}
