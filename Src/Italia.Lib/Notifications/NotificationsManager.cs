using System.Threading.Tasks;
using Dzaba.Utils;

namespace Italia.Lib.Notifications
{
    public interface INotificationsManager
    {
        Task NotifyAsync(OffersToNotify offers);
    }

    internal sealed class NotificationsManager : INotificationsManager
    {
        public async Task NotifyAsync(OffersToNotify offers)
        {
            Require.NotNull(offers, nameof(offers));

            throw new System.NotImplementedException();
        }
    }
}
