namespace Examples.Collections.Compatibility.Tests;

/// <summary>
/// Tests for <see cref="PriorityQueue{TElement, TPriority}" /> class.
/// </summary>
public class PriorityQueueTests
{
    [Fact]
    public void WhenUsingPriorityQueue_IsCompatible()
    {
        var original = new global::System.Collections.Generic.PriorityQueue<string, int>();
        original.Enqueue("First element", 200);
        original.Enqueue("Second element", 100);
        original.Enqueue("Third element", 100);
        original.Enqueue("Fourth element", 1);
        original.Enqueue("Fifth element", 300);

        var queue = new PriorityQueue<string, int>();
        queue.Enqueue("First element", 200);
        queue.Enqueue("Second element", 100);
        queue.Enqueue("Third element", 100);
        queue.Enqueue("Fourth element", 1);
        queue.Enqueue("Fifth element", 300);

        //do and assert.
        queue.Count.Is(5);
        queue.Count.Is(original.Count);

        string? actual;
        actual = queue.Dequeue();
        actual.Is("Fourth element");
        actual.Is(original.Dequeue());

        actual = queue.Dequeue();
        actual.Is("Third element");     // LIFO.
        actual.Is(original.Dequeue());

        actual = queue.Dequeue();
        actual.Is("Second element");    // LIFO.
        actual.Is(original.Dequeue());

        actual = queue.Dequeue();
        actual.Is("First element");
        actual.Is(original.Dequeue());

        actual = queue.Dequeue();
        actual.Is("Fifth element");
        actual.Is(original.Dequeue());

        return;
    }


}
