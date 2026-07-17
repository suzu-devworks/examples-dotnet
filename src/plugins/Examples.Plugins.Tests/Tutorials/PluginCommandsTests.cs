using System.Reflection;
using Examples.Plugins.Tutorials;

namespace Examples.Plugins.Tests.Tutorials;

public class PluginCommandsTests
{
    private static ITestOutputHelper? Output => TestContext.Current.TestOutputHelper;

    private static readonly string[] SourceArray = [
        @"./plugins/Examples.Plugins.Hello/Examples.Plugins.Hello.dll",
        @"./plugins/Examples.Plugins.Json/Examples.Plugins.Json.dll",
        @"./plugins/Examples.Plugins.Libuv/Examples.Plugins.Libuv.dll",
    ];

    [Fact]
    public void When_PluginIsLoaded_Then_ExecutesSuccessfully()
    {
        // Given
        var root = Path.GetDirectoryName(typeof(PluginCommandsTests).Assembly.Location)!;
        IEnumerable<ICommand> commands = SourceArray
            .Select(path => Path.GetFullPath(Path.Combine(root, path)))
            .SelectMany(pluginPath =>
            {
                Assembly pluginAssembly = LoadPlugin(pluginPath);
                return CreateCommands(pluginAssembly);
            }).ToList();

        Output?.WriteLine("Commands: ");
        foreach (ICommand command in commands)
        {
            Output?.WriteLine($" {command.Name}\t - {command.Description}");
        }

        // When
        List<(string Name, int ExitCode)> results = new();
        using TextWriter outputWriter = new StringWriter();

        foreach (ICommand command in commands)
        {
            var result = command.Execute(outputWriter);
            results.Add((command.Name, result));
        }

        // Then
        Assert.Equal(SourceArray.Length, commands.Count());
        Assert.Equal(SourceArray.Length, results.Count(x => x.ExitCode == 0));

        var output = outputWriter.ToString();
        Assert.Contains("Hello !!!", output);
        Assert.Contains("\"JsonVersion\": \"Newtonsoft.Json, Version=", output);
        Assert.Contains("Using libuv version ", output);

        static Assembly LoadPlugin(string pluginLocation)
        {
            Console.WriteLine($"Loading commands from: {pluginLocation}");
            PluginLoadContext loadContext = new(pluginLocation);
            return loadContext.LoadFromAssemblyName(new(Path.GetFileNameWithoutExtension(pluginLocation)));
        }

        static IEnumerable<ICommand> CreateCommands(Assembly assembly)
        {
            int count = 0;

            foreach (var type in assembly.GetTypes().Where(t => typeof(ICommand).IsAssignableFrom(t)))
            {
                if (Activator.CreateInstance(type) is ICommand result)
                {
                    count++;
                    yield return result;
                }
            }

            if (count == 0)
            {
                string availableTypes = string.Join(",", assembly.GetTypes().Select(t => t.FullName));
                throw new ApplicationException(
                    $"Can't find any type which implements ICommand in {assembly} from {assembly.Location}.\n" +
                    $"Available types: {availableTypes}");
            }
        }
    }
}
