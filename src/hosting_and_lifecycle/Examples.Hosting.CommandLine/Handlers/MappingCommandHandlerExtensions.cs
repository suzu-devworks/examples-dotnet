using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Examples.Hosting.CommandLine.Handlers;

/// <summary>
/// Extension methods for mapping command handlers to commands in the command tree.
/// </summary>
public static class MappingCommandHandlerExtensions
{
    /// <summary>
    /// Maps command handlers to commands in the command tree,
    /// using the provided configuration action to specify which handlers to map to which commands.
    /// </summary>
    /// <param name="host">The host instance providing dependency injection and service resolution.</param>
    /// <param name="rootCommand">The root command of the command tree to which handlers will be mapped.</param>
    /// <param name="configure">An action to configure the mapping of command handlers to commands.</param>
    public static void MapCommandHandlers(this IHost host,
        RootCommand rootCommand,
        Action<HandlerBuilder> configure)
    {
        var builder = new HandlerBuilder(host, rootCommand);
        configure(builder);
        builder.Build();
    }

    public sealed class HandlerBuilder(IHost host, RootCommand rootCommand)
    {
        private readonly Dictionary<Type, Command> _allCommands = rootCommand.GetAllCommandsRecursive()
                .Where(c => c.GetType() != typeof(Command)) // Filter out the base Command type, which is not a concrete command in the tree.
                .ToDictionary(c => c.GetType());

        private readonly Dictionary<Type, Action> _registrations = [];

        public void Build()
        {
            foreach (var registration in _registrations.Values)
            {
                registration();
            }
        }

        public void AddHandler<TCommand, THandler>()
            where TCommand : Command
            where THandler : ICommandHandler<TCommand>
        {
            _registrations.TryAdd(typeof(TCommand), RegisterHandler<TCommand, THandler>);
        }

        public void RegisterHandler<TCommand, THandler>()
            where TCommand : Command
            where THandler : ICommandHandler<TCommand>
        {
            if (!_allCommands.TryGetValue(typeof(TCommand), out var cmd))
            {
                throw new InvalidOperationException($"No command of type {typeof(TCommand).FullName} found in the command tree.");
            }

            cmd.SetAction(async (parseResult, cancellationToken) =>
            {
                using var scope = host.Services.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<THandler>();

                await handler.InvokeAsync((TCommand)cmd, parseResult, cancellationToken);
            });
        }
    }

    private static IEnumerable<Command> GetAllCommandsRecursive(this Command root)
    {
        yield return root;
        foreach (var sub in root.Subcommands)
        {
            foreach (var descendant in GetAllCommandsRecursive(sub))
            {
                yield return descendant;
            }
        }
    }
}

