
namespace Examples.Hosting.Console.Applications;

/// <summary>
/// Executes an action at specified intervals during countdown.
/// </summary>
public readonly struct Countdown
{
    private readonly int _count;

    private Countdown(int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 0, nameof(count));
        _count = count;
    }

    /// <summary>
    /// Creates a Countdown instance with the specified count.
    /// </summary>
    /// <param name="count">The starting count value.</param>
    /// <returns>A new Countdown instance.</returns>
    public static Countdown From(int count) => new(count);

    /// <summary>
    /// Sets the interval for action execution during countdown.
    /// </summary>
    /// <param name="interval">The time interval between executions.</param>
    /// <param name="timeProvider">Optional time provider. Defaults to TimeProvider.System.</param>
    /// <returns>A configured CountdownExecutor.</returns>
    public Executor Every(TimeSpan interval, TimeProvider? timeProvider = null)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(interval, TimeSpan.Zero, nameof(interval));
        return new(_count, interval, timeProvider ?? TimeProvider.System);
    }

    public Task DoAsync(Action<int> action, CancellationToken cancellationToken = default)
    {
        return Every(TimeSpan.FromSeconds(1))
            .DoAsync(action, cancellationToken);
    }

    public readonly struct Executor(int count, TimeSpan interval, TimeProvider timeProvider)
    {
        /// <summary>
        /// Executes the action at each interval until countdown reaches zero.
        /// </summary>
        /// <param name="action">The action to execute, receiving the current count.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task representing the countdown operation.</returns>
        public async Task DoAsync(Action<int> action, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(action);

            for (int i = count; i >= 0; i--)
            {
                cancellationToken.ThrowIfCancellationRequested();

                action(i);

                if (i > 0)
                {
                    await Task.Delay(interval, timeProvider, cancellationToken);
                }
            }
        }
    }
}

