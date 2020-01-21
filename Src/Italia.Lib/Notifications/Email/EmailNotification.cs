using Dzaba.Utils;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Italia.Lib.Notifications.Email
{
    internal sealed class EmailNotification : INotification
    {
        private readonly IEmailNotificationSettings settings;
        private readonly ILogger<EmailNotification> logger;
        private readonly IEmailBodyBuilder builder;

        public EmailNotification(IEmailNotificationSettings settings,
            ILogger<EmailNotification> logger,
            IEmailBodyBuilder builder)
        {
            Require.NotNull(settings, nameof(settings));
            Require.NotNull(logger, nameof(logger));
            Require.NotNull(builder, nameof(builder));

            this.logger = logger;
            this.settings = settings;
            this.builder = builder;
        }

        public async Task NotifyAsync(OffersToNotify offers)
        {
            Require.NotNull(offers, nameof(offers));

            using (var smtp = new SmtpClient(settings.SmtpHost, settings.SmtpPort))
            {
                smtp.Credentials = new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword);
                smtp.EnableSsl = settings.SmtpUseSsl;

                using (var msg = new MailMessage())
                {
                    msg.Subject = settings.EmailSubject;
                    msg.From = new MailAddress(settings.EmailFrom, settings.EmailFromDisplay);

                    msg.To.AddRange(settings.EmailTo.Select(t => new MailAddress(t)));

                    var body = builder.Generate(offers);
                    msg.Body = body.Body;
                    msg.IsBodyHtml = body.IsHtml;

                    await smtp.SendMailAsync(msg);
                }
            }
        }
    }
}
