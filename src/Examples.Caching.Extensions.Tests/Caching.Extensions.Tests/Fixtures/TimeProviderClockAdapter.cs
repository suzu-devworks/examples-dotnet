using Microsoft.Extensions.Internal;

namespace Examples.Caching.Extensions.Tests.Fixtures;

/// <summary>
/// Adapter class that implements ISystemClock interface and uses TimeProvider to get the current time.
/// </summary>
/// <param name="timeProvider"></param>
public class TimeProviderClockAdapter(TimeProvider timeProvider) : ISystemClock
{
    private readonly TimeProvider _timeProvider = timeProvider;

    // Get the current time required by ISystemClock from TimeProvider and return it
    public DateTimeOffset UtcNow => _timeProvider.GetUtcNow();
}
