using System;
using System.IO;
using Dzaba.Utils;
using Italia.Lib.Dal;
using Italia.Lib.Notifications;
using Italia.Lib.Notifications.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Italia.Lib
{
    public static class Bootstrapper
    {
        private static readonly string ConnectionString = $"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.db")}";

        public static void RegisterItalia(this IServiceCollection container)
        {
            Require.NotNull(container, nameof(container));

            container.AddTransient<IItaliaEngine, ItaliaEngine>();
            container.AddTransient<IOffersDal, OffersDal>();
            container.AddTransient<INotificationsManager, NotificationsManager>();
            container.AddTransient<INotification, EmailNotification>();
            container.AddTransient<IEmailBodyBuilder, TextEmailBodyBuilder>();
            container.AddTransient<IDbInitializer, DbInitializer>();
            container.AddTransient(BuildConfiguration);
            container.AddTransient<IEmailNotificationSettings>(c => GetSettings<EmailNotificationSettings>(c, nameof(EmailNotificationSettings)));

            container.AddDbContext<DatabaseContext>(o => o.UseSqlite(ConnectionString), ServiceLifetime.Transient,
                ServiceLifetime.Transient);
            container.AddTransient<Func<DatabaseContext>>(c => c.GetRequiredService<DatabaseContext>);
        }

        private static IConfiguration BuildConfiguration(IServiceProvider container)
        {
            var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appSettings.json");

            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(filename);

            return configBuilder.Build();
        }

        private static T GetSettings<T>(IServiceProvider container, string sectionName)
            where T : new()
        {
            var configuration = container.GetRequiredService<IConfiguration>();

            var settings = new T();
            configuration.Bind(sectionName, settings);

            return settings;
        }
    }
}
