namespace Examples.Concurrency.Tests;

[Collection(TestCollectionNames.UseThread)]
public class ThreadAggregatorTests
{
    private static readonly ITestOutputHelper? Output = TestContext.Current?.TestOutputHelper;

    [Fact]
    public void When_Start4ThreadSimultaneously_Then_TerminatesAsExpected()
    {
        var aggregator = new ThreadAggregator(maxThreads: 4)
            .AddWorker(() => { }, () => Output?.WriteLine("Thread 1 completed"))
            .AddWorker(() => { }, () => Output?.WriteLine("Thread 2 completed"))
            .AddWorker(() => { }, () => Output?.WriteLine("Thread 3 completed"))
            .AddWorker(() => { }, () => Output?.WriteLine("Thread 4 completed"));

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
            .AddWorker(() => { }, () => Output?.WriteLine("Thread 1 completed"))
            .AddWorker(() => { }, () => Output?.WriteLine("Thread 2 completed"))
            .AddWorker(() => { }, () => Output?.WriteLine("Thread 3 completed"));

        aggregator.AddWorker(
            () => Assert.True(aggregator.CompletedCount > 0),
            () => Output?.WriteLine("Thread 4 completed"));

        aggregator.StartAll();

        aggregator.WaitAll();

        Assert.Equal(0, aggregator.RunningCount);
        Assert.Equal(4, aggregator.CompletedCount);
        Assert.Empty(aggregator.Exceptions);
    }

}
