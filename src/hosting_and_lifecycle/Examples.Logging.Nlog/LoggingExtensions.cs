namespace Examples.Logging.Nlog;

public static partial class LoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Worker is starting at: {time:o}")]
    public static partial void LogWorkerStarting(this ILogger logger, DateTimeOffset time);

    [LoggerMessage(2, LogLevel.Information, "Worker is stopping at: {time:o}")]
    public static partial void LogWorkerStopping(this ILogger logger, DateTimeOffset time);

    [LoggerMessage(3, LogLevel.Information, "Worker is running at: {time:o}")]
    public static partial void LogWorkerRunning(this ILogger logger, DateTimeOffset time);

}
