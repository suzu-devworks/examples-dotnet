using System.Reflection;
using Examples.Plugins.Tutorials;
using Newtonsoft.Json;

namespace Examples.Plugins.Json;

public class JsonCommand : ICommand
{
    public string Name => "json";

    public string Description => "Outputs JSON value.";

    private struct Info
    {
        public string? JsonVersion;
        public string? JsonLocation;
        public string Machine;
        public string User;
        public DateTime Date;
    }

    public int Execute(TextWriter output)
    {
        Assembly jsonAssembly = typeof(JsonConvert).Assembly;
        Info info = new Info()
        {
            JsonVersion = jsonAssembly?.FullName,
            JsonLocation = jsonAssembly?.Location,
            Machine = Environment.MachineName,
            User = Environment.UserName,
            Date = DateTime.Now
        };

        output.WriteLine(JsonConvert.SerializeObject(info, Formatting.Indented));

        return 0;
    }
}
