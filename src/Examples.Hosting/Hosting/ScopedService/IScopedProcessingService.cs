namespace Examples.Hosting.ScopedService;

/// <summary>
/// Provides an interface for scoped services used by background services.
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/dotnet/core/extensions/scoped-service?pivots=dotnet-6-0" />
public interface IScopedProcessingService
{
    /// <summary>
    /// Defines methods for objects.
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    Task DoWorkAsync(CancellationToken stoppingToken);
}
