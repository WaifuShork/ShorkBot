using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;
using ShorkBot.Services;
using TwitchLib.Client;

namespace ShorkBot
{
    public static class Startup
    {
        private static async Task<int> Main(string[] args)
        {
            return await RunAsync(args).ConfigureAwait(false);
        }
            
        private static async Task<int> RunAsync(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting ShorkBot");
                await CreateDefaultBuilder(args).Build().RunAsync().ConfigureAwait(false);
                return 1;
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        
        private static IHostBuilder CreateDefaultBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureAppConfiguration(x =>
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .Build();
                    
                    x.AddConfiguration(configuration);
                })
                .ConfigureServices((_, services) =>
                {
                    services.AddHostedService<Bot>();
                    services.AddHostedService<FilterService>();
                    services.AddSingleton<TwitchClient>();
                    services.AddSingleton<BotConfiguration>();
                })
                .UseConsoleLifetime();
        }
    }
}