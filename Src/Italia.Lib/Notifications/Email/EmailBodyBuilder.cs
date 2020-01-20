namespace Italia.Lib.Notifications.Email
{
    public interface IEmailBodyBuilder
    {
        string Generate(OffersToNotify offers);
    }

    internal sealed class EmailBodyBuilder : IEmailBodyBuilder
    {
        public string Generate(OffersToNotify offers)
        {
            throw new System.NotImplementedException();
        }
    }
}
