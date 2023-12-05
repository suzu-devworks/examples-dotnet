namespace Examples.Collections.Tests;

/// <summary>
/// Tests <see cref="DelegationEqualityComparer{T}" /> class.
/// </summary>
public class DelegationEqualityComparerTests
{

    [Fact]
    public void WhenCallingLinqDistinct_ReturnsAsExpected()
    {
        var inputs = Enumerable.Range(1, 10)
            .Select(n => new Data { Value = n % 3 });

        var actual = inputs
            .Distinct(new DelegationEqualityComparer<Data>((x, y) => x.Value == y.Value))
            .OrderBy(x => x.Value)
            .Select(x => x.Value)
            .ToArray();

        actual.Is([0, 1, 2]);
    }

    private class Data
    {
        public int Value { get; init; }
    }

}
