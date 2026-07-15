namespace Examples.Hosting.Applications.TimerService;

/// <summary>
/// Provides background services triggered by timers.
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/dotnet/core/extensions/timer-service?pivots=dotnet-6-0" />
public sealed class TimerHostedService : IHostedService, IAsyncDisposable
{
    private readonly Task _completedTask = Task.CompletedTask;
    private readonly ILogger<TimerHostedService> _logger;
    private int _executionCount = 0;
    private Timer? _timer;

    /// <summary>
    /// Creates a new instance of the <see cref="TimerHostedService"/>.
    /// </summary>
    /// <param name="logger"></param>
    public TimerHostedService(
        ILogger<TimerHostedService> logger)
        => _logger = logger;

    /// <inheritdoc/>
    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("{Service} is running.", nameof(TimerHostedService));
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        return _completedTask;
    }

    private void DoWork(object? state)
    {
        int count = Interlocked.Increment(ref _executionCount);

        _logger.LogInformation(
            "{Service} is working, execution count: {Count:#,0}",
            nameof(TimerHostedService),
            count);
    }

    /// <inheritdoc/>
    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "{Service} is stopping.", nameof(TimerHostedService));

        _timer?.Change(Timeout.Infinite, 0);

        return _completedTask;
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (_timer is IAsyncDisposable timer)
        {
            await timer.DisposeAsync();
        }

        _timer = null;
    }
}
