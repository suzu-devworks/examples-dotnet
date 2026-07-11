namespace Examples.Management.Tests;

public partial class HashCodeCalculatorTests
{
    private record DataClass(int Field1, string? Field2, DateTime? Field3);

    private static void RunEqualTest(
        Management.HashCodeCalculator.IHashCodeCalculator hashCodeCalculator,
        DataClass instance1,
        DataClass instance2)
    {
        // Act
        var actual1 = hashCodeCalculator.GetHashCode(instance1.Field1, instance1.Field2, instance1.Field3);
        var actual2 = hashCodeCalculator.GetHashCode(instance2.Field1, instance2.Field2, instance2.Field3);

        // Assert
        Assert.Equal(actual1, actual2);

        TestContext.Current.TestOutputHelper?.WriteLine($"Calculated hash code for instance1: {actual1}");
        TestContext.Current.TestOutputHelper?.WriteLine($"Record original hash code for instance1: {instance1.GetHashCode()}");
    }

    private static void RunNotEqualTest(
        Management.HashCodeCalculator.IHashCodeCalculator hashCodeCalculator,
        DataClass instance1,
        DataClass instance2)
    {
        // Act
        var actual1 = hashCodeCalculator.GetHashCode(instance1.Field1, instance1.Field2, instance1.Field3);
        var actual2 = hashCodeCalculator.GetHashCode(instance2.Field1, instance2.Field2, instance2.Field3);

        // Assert
        Assert.NotEqual(actual1, actual2);

        TestContext.Current.TestOutputHelper?.WriteLine($"Calculated hash code instance1: {actual1}, instance2: {actual2}");
        TestContext.Current.TestOutputHelper?.WriteLine($"Record original hash instance1: {instance1.GetHashCode()}, instance2: {instance2.GetHashCode()}");
    }

    private static void RunEmptyTest(Management.HashCodeCalculator.IHashCodeCalculator hashCodeCalculator)
    {
        // Act
        var actual1 = hashCodeCalculator.GetHashCode();
        var actual2 = hashCodeCalculator.GetHashCode();

        // Assert
        Assert.Equal(actual1, actual2);

        TestContext.Current.TestOutputHelper?.WriteLine($"Calculated hash code for instance1: {actual1}");
    }
}
