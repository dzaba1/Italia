using System;
using Italia.Lib;
using Microsoft.Extensions.DependencyInjection;

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
