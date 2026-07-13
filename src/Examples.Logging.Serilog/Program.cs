using Examples.Logging.Serilog;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    // .CreateLogger();
    .CreateBootstrapLogger(); // <-- Change this line!

try
{
    Log.Information("Starting host");

    var builder = Host.CreateApplicationBuilder(args);

    // Serilog can be configured from appsettings.json. Serilog.Settings.Configuration is required.
    builder.Services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
           .ReadFrom.Configuration(builder.Configuration)
           .ReadFrom.Services(services)
           // If you define it in appsettings.json, it will be output twice, so comment it out if you define it in appsettings.json.
           // .WriteTo.Console()
           // .Enrich.FromLogContext()
           );

    builder.Services.AddHostedService<Worker>();

    var app = builder.Build();

    // SinceMinimumLevel = 'Debug' is set in `appsettings.json`, logs will not be output if it is not enabled.
    Log.Debug("appsettings.json loaded with MinimumLevel(Default)={value}",
        builder.Configuration.GetValue<string>("Serilog:MinimumLevel:Default"));

    await app.RunAsync();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}
