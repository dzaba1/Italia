namespace Italia.Lib.Notifications.Email
{
    public interface IEmailNotificationSettings
    {
        string SmtpHost { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; }
        string SmtpPassword { get; }
        bool SmtpUseSsl { get; }
        string EmailSubject { get; }
        string EmailFromDisplay { get; }
        string EmailFrom { get; }
        string[] EmailTo { get; }
    }
}
