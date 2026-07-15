namespace Examples.Plugins.Tutorials;

public interface ICommand
{
    string Name { get; }
    string Description { get; }
    int Execute(TextWriter output);
}
