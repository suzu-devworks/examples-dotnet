namespace Examples.Hosting.Console.Applications;

public interface IRunner
{
    Task RunAsync(string param, CancellationToken cancelToken = default);

}
