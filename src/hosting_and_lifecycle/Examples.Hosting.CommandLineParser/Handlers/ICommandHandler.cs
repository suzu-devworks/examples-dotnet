namespace Examples.Hosting.CommandLineParser.Handlers;

/// <summary>
/// Defines a contract for handling a specific command type.
/// </summary>
public interface ICommandHandler
{
    /// <summary>
    /// Executes the command's logic asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}
