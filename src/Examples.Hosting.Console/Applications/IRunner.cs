namespace Examples.Hosting.Console.Applications;

public interface IRunner
{
    public Task RunAsync(string param, CancellationToken cancelToken = default);

}
