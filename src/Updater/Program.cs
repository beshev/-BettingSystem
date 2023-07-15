using Infrastructure.Extentions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Updater;

var builder = new HostBuilder()
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false);
        config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services
        .AddHostedService<ApplicationService>()
        .AddDatabase(hostContext.Configuration)
        .AddServices()
        .AddAutoMapper()
        .AddEventTriggers()
        .AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Warning);
        });
    });

await builder.RunConsoleAsync();