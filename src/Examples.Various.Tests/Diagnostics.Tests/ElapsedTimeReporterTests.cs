namespace Examples.Diagnostics.Tests;

/// <summary>
/// Tests <see cref="ElapsedTimeReporter" /> class.
/// </summary>
public class ElapsedTimeReporterTests
{

    [Fact]
    public void WhenUsingSimply_ReturnsAsExpected()
    {
        TimeSpan? reportTime = null;
        using (var reporter = ElapsedTimeReporter.Start(x => reportTime = x))
        {
            // any codes.

        } // Stop Elapsed Time in Dispose().

        // Assert
        reportTime.IsNotNull();

        return;
    }


    [Fact]
    public void WhenCallingSimply_StopwatchIsStoppedOnDispose()
    {
        // Given
        var reporter = ElapsedTimeReporter.Start(x => { });

        reporter.IsRunning.IsTrue();

        // When
        reporter.Dispose();

        // Then
        reporter.IsRunning.IsFalse();

        return;
    }

}
