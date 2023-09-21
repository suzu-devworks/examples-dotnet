namespace Examples.Hosting.QueueService;

/// <summary>
/// Provides an interface for queue used by background service.
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/dotnet/core/extensions/queue-service?pivots=dotnet-6-0"/>
public interface IBackgroundTaskQueue
{
    /// <summary>
    /// Schedules a task which can run in the background, independent of any request.
    /// </summary>
    /// <param name="workItem">A unit of execution.</param>
    /// <returns></returns>
    ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, ValueTask> workItem);

    /// <summary>
    /// Gets a task whose result is the element at the head of the queue.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);

}
