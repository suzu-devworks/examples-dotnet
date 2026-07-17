using Examples.Logging.Nlog;
using NLog;
using NLog.Extensions.Hosting;
using NLog.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

var logger = NLog.LogManager.Setup()
    .LoadConfigurationFromSection(builder.Configuration)
    .GetCurrentClassLogger();

try
{
    builder.Logging.ClearProviders();
    // builder.Logging.AddNLog();
    builder.UseNLog();

    builder.Services.AddHostedService<Worker>();

    await builder.Build().RunAsync();
}
catch (Exception ex)
{
    //NLog: catch setup errors
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}

