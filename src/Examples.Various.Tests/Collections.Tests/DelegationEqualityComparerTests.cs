namespace Examples.Collections.Tests;

/// <summary>
/// Tests <see cref="DelegationEqualityComparer{T}" /> class.
/// </summary>
public class DelegationEqualityComparerTests
{
    private class DataClass
    {
        public int Value1 { get; init; }
        public string? Value2 { get; init; }
    }

    [Fact]
    public void When_UsingParameterForLinqDistinct_Then_ReturnsUniqueBasedOnComparisonOfMembers()
    {
        var inputs = Enumerable.Range(1, 10)
                        .Select(n => new DataClass { Value1 = n % 3, Value2 = $"Value {n}" });

        var sequence = from w in inputs.Distinct(
                            new DelegationEqualityComparer<DataClass>((x, y) => x.Value1 == y.Value1))
                       orderby w.Value1
                       select w;
        var actual = sequence.ToArray();

        Assert.Equal(3, actual.Length);
        Assert.Equal(0, actual[0].Value1);
        Assert.Equal(1, actual[1].Value1);
        Assert.Equal(2, actual[2].Value1);
    }
}
