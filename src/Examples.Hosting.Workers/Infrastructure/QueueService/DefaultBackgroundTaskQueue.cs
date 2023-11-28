using System.Threading.Channels;

namespace Examples.Hosting.Infrastructure.QueueService;

/// <summary>
/// Default implementation of <see cref="IBackgroundTaskQueue"/>
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/dotnet/core/extensions/queue-service?pivots=dotnet-6-0"/>
public sealed class DefaultBackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

    /// <summary>
    /// Creates a new instance of the <see cref="DefaultBackgroundTaskQueue"/>.
    /// </summary>
    /// <param name="capacity"></param>
    public DefaultBackgroundTaskQueue(int capacity)
    {
        BoundedChannelOptions options = new(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
    }

    /// <inheritdoc/>
    public async ValueTask QueueBackgroundWorkItemAsync(
        Func<CancellationToken, ValueTask> workItem)
    {
        if (workItem is null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }

        await _queue.Writer.WriteAsync(workItem);
    }

    /// <inheritdoc/>
    public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(
        CancellationToken cancellationToken)
    {
        Func<CancellationToken, ValueTask>? workItem =
            await _queue.Reader.ReadAsync(cancellationToken);

        return workItem;
    }
}
