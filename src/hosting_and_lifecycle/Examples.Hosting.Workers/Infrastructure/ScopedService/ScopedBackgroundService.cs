namespace Examples.Hosting.Infrastructure.ScopedService;

/// <summary>
/// Provides a background service that internally creates explicit scopes and resolves dependencies.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/core/extensions/scoped-service?pivots=dotnet-6-0"/>
public class ScopedBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ScopedBackgroundService> _logger;

    /// <summary>
    /// Creates a new instance of the <see cref="ScopedBackgroundService"/>.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public ScopedBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<ScopedBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"{nameof(ScopedBackgroundService)} is running.");

        await DoWorkAsync(stoppingToken);
    }

    private async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"{nameof(ScopedBackgroundService)} is working.");

        using IServiceScope scope = _serviceProvider.CreateScope();
        IScopedProcessingService scopedProcessingService =
            scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

        await scopedProcessingService.DoWorkAsync(stoppingToken);
    }

    /// <inheritdoc/>
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"{nameof(ScopedBackgroundService)} is stopping.");

        await base.StopAsync(stoppingToken);
    }
}
