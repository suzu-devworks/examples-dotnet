namespace Examples.Concurrency.Tests;

[Collection(TestCollectionNames.UseThread)]
public class ThreadAggregatorTests
{
    private readonly ITestOutputHelper? _output = TestContext.Current?.TestOutputHelper;

    [Fact]
    public void When_Start4ThreadSimultaneously_Then_TerminatesAsExpected()
    {
        var aggregator = new ThreadAggregator(maxThreads: 4)
            .AddWorker(() => { }, () => _output?.WriteLine("Thread 1 completed"))
            .AddWorker(() => { }, () => _output?.WriteLine("Thread 2 completed"))
            .AddWorker(() => { }, () => _output?.WriteLine("Thread 3 completed"))
            .AddWorker(() => { }, () => _output?.WriteLine("Thread 4 completed"));

        aggregator.StartAll();

        aggregator.WaitAll();

        Assert.Equal(0, aggregator.RunningCount);
        Assert.Equal(4, aggregator.CompletedCount);
        Assert.Empty(aggregator.Exceptions);
    }

    [Fact]
    public void When_AddMoreWorkerThenNumberOfThreads_Then_TerminatesAsExpected()
    {
        var aggregator = new ThreadAggregator(maxThreads: 3)
            .AddWorker(() => { }, () => _output?.WriteLine("Thread 1 completed"))
            .AddWorker(() => { }, () => _output?.WriteLine("Thread 2 completed"))
            .AddWorker(() => { }, () => _output?.WriteLine("Thread 3 completed"));

        aggregator.AddWorker(
            () => Assert.True(aggregator.CompletedCount > 0),
            () => _output?.WriteLine("Thread 4 completed"));

        aggregator.StartAll();

        aggregator.WaitAll();

        Assert.Equal(0, aggregator.RunningCount);
        Assert.Equal(4, aggregator.CompletedCount);
        Assert.Empty(aggregator.Exceptions);
    }

}
