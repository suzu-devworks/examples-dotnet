namespace Examples.Hosting.Console.Applications;

public interface IRunner
{
    public Task RunAsync(string[] args, CancellationToken cancelToken = default);

}
