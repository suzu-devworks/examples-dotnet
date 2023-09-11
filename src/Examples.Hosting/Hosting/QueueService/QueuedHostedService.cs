using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Examples.Hosting.QueueService;

/// <summary>
/// Provides a background service that runs throught a Queue.
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/dotnet/core/extensions/queue-service?pivots=dotnet-6-0"/>
public class QueuedHostedService : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<QueuedHostedService> _logger;

    /// <summary>
    /// Creates a new instance of the <see cref="QueuedHostedService"/>.
    /// </summary>
    /// <param name="taskQueue"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public QueuedHostedService(
        IBackgroundTaskQueue taskQueue,
        ILogger<QueuedHostedService> logger) =>
        (_taskQueue, _logger) = (taskQueue, logger);

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int maxConcurrency = 2;
        _logger.LogInformation("{name} is running with concurrency {num}.",
            nameof(QueuedHostedService), maxConcurrency);

        IEnumerable<Task> tasks = Enumerable.Range(0, maxConcurrency)
            .Select(async _ => await ProcessTaskQueueAsync(stoppingToken));

        await Task.WhenAll(tasks);
        return;
    }

    private async Task ProcessTaskQueueAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var workItem = await _taskQueue.DequeueAsync(stoppingToken);

                await workItem(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if stoppingToken was signaled
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing task work item.");
            }
        }
    }

    /// <inheritdoc/>
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"{nameof(QueuedHostedService)} is stopping.");

        await base.StopAsync(stoppingToken);
    }
}
