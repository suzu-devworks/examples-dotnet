using System.CommandLine;

namespace Examples.Hosting.CommandLine.Handlers;

/// <summary>
/// Defines a contract for handling a specific command type.
/// </summary>
/// <typeparam name="TCommand">The type of command handled by the handler.</typeparam>
public interface ICommandHandler<in TCommand> where TCommand : Command
{
    /// <summary>
    /// Invokes the handler for the specified command, using the provided parse result and cancellation token.
    /// </summary>
    /// <param name="command">The command instance to be handled.</param>
    /// <param name="parseResult">The result of parsing the command line input.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The exit code returned by the handler.</returns>
    Task<int> InvokeAsync(TCommand command, ParseResult parseResult, CancellationToken cancellationToken = default);
}
