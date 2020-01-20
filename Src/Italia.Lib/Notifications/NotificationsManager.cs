using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dzaba.Utils;
using Microsoft.Extensions.Logging;

namespace Italia.Lib.Notifications
{
    public interface INotificationsManager
    {
        Task NotifyAsync(OffersToNotify offers);
    }

    internal sealed class NotificationsManager : INotificationsManager
    {
        private readonly INotification[] notifications;
        private readonly ILogger<NotificationsManager> logger;

        public NotificationsManager(IEnumerable<INotification> notifications,
            ILogger<NotificationsManager> logger)
        {
            Require.NotNull(notifications, nameof(notifications));
            Require.NotNull(logger, nameof(logger));

            this.logger = logger;
            this.notifications = notifications.ToArray();
        }

        public async Task NotifyAsync(OffersToNotify offers)
        {
            Require.NotNull(offers, nameof(offers));

            foreach (var notification in notifications)
            {
                logger.LogInformation($"Calling notification {notification.GetType()}");
                await notification.NotifyAsync(offers);
            }
        }
    }
}
