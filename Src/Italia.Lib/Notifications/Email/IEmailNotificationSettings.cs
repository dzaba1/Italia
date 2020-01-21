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

    public sealed class EmailNotificationSettings : IEmailNotificationSettings
    {
        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public string SmtpUsername { get; set; }

        public string SmtpPassword { get; set; }

        public bool SmtpUseSsl { get; set; }

        public string EmailSubject { get; set; }

        public string EmailFromDisplay { get; set; }

        public string EmailFrom { get; set; }

        public string[] EmailTo { get; set; }
    }
}
