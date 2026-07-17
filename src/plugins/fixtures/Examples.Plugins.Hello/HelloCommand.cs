using Examples.Plugins.Tutorials;

namespace Examples.Plugins.Hello;

public class HelloCommand : ICommand
{
    public string Name { get => "hello"; }
    public string Description { get => "Displays hello message."; }

    public int Execute(TextWriter output)
    {
        output.WriteLine("Hello !!!");
        return 0;
    }
}
