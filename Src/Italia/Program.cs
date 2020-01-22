using System;
using System.IO;
using Italia.Lib;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Italia
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var services = new ServiceCollection();
                services.RegisterItalia();
                services.AddTransient<IApp, App>();

                var logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .WriteTo.RollingFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Italia.log"), retainedFileCountLimit: 15)
                    .CreateLogger();
                services.AddLogging(logBuilder => logBuilder.AddSerilog(logger, true));

                var container = services.BuildServiceProvider();

                using (var app = container.GetRequiredService<IApp>())
                {
                    app.RunAsync().Wait();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.ExitCode = 1;
                throw;
            }
        }
    }
}
