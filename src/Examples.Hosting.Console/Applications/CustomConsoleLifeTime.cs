using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Examples.Hosting.Console.Applications;

/// <summary>
/// An implementation to understand what <see cref="IHostLifetime"/> does.
/// Currently, it's not doing anything in particular.
/// </summary>
public class CustomConsoleLifeTime : IHostLifetime, IDisposable
{
    private CancellationTokenRegistration _applicationStartedRegistration;
    private CancellationTokenRegistration _applicationStoppingRegistration;
    private CancellationTokenRegistration _applicationStoppedRegistration;

    public CustomConsoleLifeTime(
        IOptions<ConsoleLifetimeOptions> options,
        IHostEnvironment environment,
        IHostApplicationLifetime applicationLifetime,
        IOptions<HostOptions> hostOptions)
        : this(options, environment, applicationLifetime, hostOptions, NullLoggerFactory.Instance) { }

    public CustomConsoleLifeTime(
        IOptions<ConsoleLifetimeOptions> options,
        IHostEnvironment environment,
        IHostApplicationLifetime applicationLifetime,
        IOptions<HostOptions> hostOptions,
        ILoggerFactory loggerFactory)
    {
        Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        ApplicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        HostOptions = hostOptions?.Value ?? throw new ArgumentNullException(nameof(hostOptions));
        Logger = loggerFactory.CreateLogger("Examples.Hosting." + nameof(CustomConsoleLifeTime));
    }

    private ConsoleLifetimeOptions Options { get; }

    private IHostEnvironment Environment { get; }

    private IHostApplicationLifetime ApplicationLifetime { get; }

    private HostOptions HostOptions { get; }

    private ILogger Logger { get; }

    public Task WaitForStartAsync(CancellationToken cancellationToken = default)
    {
        if (!Options.SuppressStatusMessages)
        {
            _applicationStartedRegistration = ApplicationLifetime.ApplicationStarted.Register(state =>
            {
                ((CustomConsoleLifeTime?)state)?.OnApplicationStarted();
            },
            this);

            _applicationStoppingRegistration = ApplicationLifetime.ApplicationStopping.Register(state =>
            {
                ((CustomConsoleLifeTime?)state)?.OnApplicationStopping();
            },
            this);

            _applicationStoppedRegistration = ApplicationLifetime.ApplicationStopped.Register(state =>
            {
                ((CustomConsoleLifeTime?)state)?.OnApplicationStopped();
            },
            this);
        }

        RegisterShutdownHandlers();

        // Console applications start immediately.
        return Task.CompletedTask;
    }

    private void OnApplicationStarted()
    {
        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation("Application started.");
            Logger.LogInformation("Hosting environment: {envName}", Environment.EnvironmentName);
            Logger.LogInformation("Content root path: {contentRoot}", Environment.ContentRootPath);
        }
    }

    private void OnApplicationStopping()
    {
        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation("Application is shutting down...");
        }
    }

    private void OnApplicationStopped()
    {
        Logger.LogInformation("Application is stopped.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // There's nothing to do here
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        UnregisterShutdownHandlers();

        _applicationStartedRegistration.Dispose();
        _applicationStoppingRegistration.Dispose();
        _applicationStoppedRegistration.Dispose();
        GC.SuppressFinalize(this);
    }

    private readonly ManualResetEvent _shutdownBlock = new ManualResetEvent(false);

    private void RegisterShutdownHandlers()
    {
        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        global::System.Console.CancelKeyPress += OnCancelKeyPress;
    }

    private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        e.Cancel = true;
        ApplicationLifetime.StopApplication();

        // Don't block in process shutdown for CTRL+C/SIGINT since we can set e.Cancel to true
        // we assume that application code will unwind once StopApplication signals the token
        _shutdownBlock.Set();
    }

    private void OnProcessExit(object? sender, EventArgs e)
    {
        ApplicationLifetime.StopApplication();
        if (!_shutdownBlock.WaitOne(HostOptions.ShutdownTimeout))
        {
            Logger.LogInformation("Waiting for the host to be disposed. Ensure all 'IHost' instances are wrapped in 'using' blocks.");
        }

        // wait one more time after the above log message, but only for ShutdownTimeout, so it doesn't hang forever
        _shutdownBlock.WaitOne(HostOptions.ShutdownTimeout);

        // On Linux if the shutdown is triggered by SIGTERM then that's signaled with the 143 exit code.
        // Suppress that since we shut down gracefully. https://github.com/dotnet/aspnetcore/issues/6526
        System.Environment.ExitCode = 0;
    }

    private void UnregisterShutdownHandlers()
    {
        _shutdownBlock.Set();

        AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
        global::System.Console.CancelKeyPress -= OnCancelKeyPress;
    }
}
