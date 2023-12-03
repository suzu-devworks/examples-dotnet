namespace Examples.Hosting.Infrastructure.ScopedService;

/// <summary>
/// Default implementation of <see cref="IScopedProcessingService"/>
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/dotnet/core/extensions/scoped-service?pivots=dotnet-6-0"/>
public sealed class DefaultScopedProcessingService : IScopedProcessingService
{
    private int _executionCount;
    private readonly ILogger<DefaultScopedProcessingService> _logger;

    /// <summary>
    /// Creates a new instance of the <see cref="DefaultScopedProcessingService"/>.
    /// </summary>
    /// <param name="logger"></param>
    public DefaultScopedProcessingService(
           ILogger<DefaultScopedProcessingService> logger) =>
           _logger = logger;

    /// <inheritdoc/>
    public async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            ++_executionCount;

            _logger.LogInformation(
                "{ServiceName} working, execution count: {Count}",
                nameof(DefaultScopedProcessingService),
                _executionCount);

            await Task.Delay(10_000, stoppingToken);
        };
    }
}
