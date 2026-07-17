namespace Examples.Fluency.Tests;

public class DateTimeExtensionsTests
{
    public class TruncateMethod
    {
        [Theory]
        [InlineData("2023-01-02T12:34:56.7891234Z", "2023-01-02T12:34:56.7891230Z", TimeSpan.TicksPerMicrosecond)]
        [InlineData("2023-01-02T12:34:56.7891234Z", "2023-01-02T12:34:56.7890000Z", TimeSpan.TicksPerMillisecond)]
        [InlineData("2023-01-02T12:34:56.7891234Z", "2023-01-02T12:34:56.0000000Z", TimeSpan.TicksPerSecond)]
        [InlineData("2023-01-02T12:34:56.7891234Z", "2023-01-02T12:34:00.0000000Z", TimeSpan.TicksPerMinute)]
        [InlineData("2023-01-02T12:34:56.7891234Z", "2023-01-02T12:00:00.0000000Z", TimeSpan.TicksPerHour)]
        [InlineData("2023-01-02T12:34:56.7891234Z", "2023-01-02T00:00:00.0000000Z", TimeSpan.TicksPerDay)]
        public void When_ValidValues_Then_ReturnsAsExpected(string input, string expected, long tickSpan)
        {
            TestWithDateTime();
            TestWithDateTimeOffset();

            void TestWithDateTime()
            {
                var inputDateTime = DateTime.Parse(input);
                var expectedDateTime = DateTime.Parse(expected);

                var truncatedDateTime = inputDateTime.Truncate(tickSpan);

                Assert.Equal(expectedDateTime, truncatedDateTime);
            }

            void TestWithDateTimeOffset()
            {
                var inputDateTimeOffset = DateTimeOffset.Parse(input);
                var expectedDateTimeOffset = DateTimeOffset.Parse(expected);

                var truncatedDateTimeOffset = inputDateTimeOffset.Truncate(tickSpan);

                Assert.Equal(expectedDateTimeOffset, truncatedDateTimeOffset);
            }
        }
    }
}
