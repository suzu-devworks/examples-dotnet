namespace Examples.Collections.Compatibility.Tests;

/// <summary>
/// Tests for <see cref="PriorityQueue{TElement, TPriority}" /> class.
/// </summary>
public class PriorityQueueTests
{
    [Fact]
    public void When_AddItemsWithAssignedPriorities_Then_RetrievesInOrderOfPriority()
    {
        // Arrange.
        var queue = new PriorityQueue<string, int>();
        foreach (var (element, priority) in GetTestData())
        {
            queue.Enqueue(element, priority);
        }

        // Act & Assert.
        Assert.Equal("Fourth element", queue.Dequeue());
        Assert.Equal("Third element", queue.Dequeue());     // LIFO.
        Assert.Equal("Second element", queue.Dequeue());    // LIFO.
        Assert.Equal("First element", queue.Dequeue());
        Assert.Equal("Fifth element", queue.Dequeue());
    }

    [Fact]
    public void When_ComparisonWithOfficialVersion_Then_ReturnsMatch()
    {
        // Arrange.
        var data = GetTestData().ToArray();

        var queue = new PriorityQueue<string, int>();
        foreach (var (element, priority) in data)
        {
            queue.Enqueue(element, priority);
        }

        var official = new global::System.Collections.Generic.PriorityQueue<string, int>();
        foreach (var (element, priority) in data)
        {
            official.Enqueue(element, priority);
        }

        // Act & Assert.
        for (var i = 0; i < data.Length; i++)
        {
            var actual = queue.Dequeue();
            var expected = official.Dequeue();
            Assert.Equal(expected, actual);
        }
    }

    private static IEnumerable<(string Element, int Priority)> GetTestData()
    {
        yield return ("First element", 200);
        yield return ("Second element", 100);
        yield return ("Third element", 100);
        yield return ("Fourth element", 1);
        yield return ("Fifth element", 300);
    }

}
