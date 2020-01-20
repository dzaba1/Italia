using System.Threading.Tasks;

namespace Italia.Lib.Notifications
{
    public interface INotification
    {
        Task NotifyAsync(OffersToNotify offers);
    }
}
