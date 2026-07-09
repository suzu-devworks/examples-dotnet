namespace Examples.Diagnostics.Tests;

/// <summary>
/// Tests <see cref="ElapsedTimeReporter" /> class.
/// </summary>
public class ElapsedTimeReporterTests
{
    [Fact]
    public void When_UsedWithUsingScope_Then_CompletionIsReported()
    {
        TimeSpan? reportTime = null;

        using (var reporter = ElapsedTimeReporter.Start(x => reportTime = x))
        {
            // any codes.

        } // Stop Elapsed Time in Dispose().

        Assert.NotNull(reportTime);
    }

    [Fact]
    public void When_DisposeCalled_Then_IsRunningBecomesFalse()
    {
        var reporter = ElapsedTimeReporter.Start(x => { });

        Assert.True(reporter.IsRunning);

        reporter.Dispose();

        Assert.False(reporter.IsRunning);
    }

    [Fact]
    public void When_ThrowExceptionInReportAction_Then_DisposingExceptionIsSet()
    {
        var reporter = ElapsedTimeReporter.Start(x => throw new InvalidOperationException("Test Exception"));

        reporter.Dispose();

        Assert.NotNull(reporter.DisposingException);
        Assert.IsType<InvalidOperationException>(reporter.DisposingException);
    }
}
