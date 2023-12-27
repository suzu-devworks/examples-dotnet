namespace Examples.Collections.Compatibility;

/// <summary>
/// Priority queue implementation (Min heap).
/// </summary>
/// <remarks>
/// <para>
///  Implemented as a heap practice with reference to the .NET 6.0 API.
/// </para>
/// </remarks>
/// <typeparam name="TElement"></typeparam>
/// <typeparam name="TPriority"></typeparam>
/// <seealso href="https://learn.microsoft.com/ja-jp/dotnet/core/whats-new/dotnet-6#priorityqueue-class" />
public class PriorityQueue<TElement, TPriority>
{
    private readonly List<(TElement Element, TPriority Priority)> _nodes = new();

    private readonly IComparer<TPriority> _comparer;

    public PriorityQueue()
        : this(Comparer<TPriority>.Default)
    {
    }

    public PriorityQueue(IComparer<TPriority> comparer)
    {
        _comparer = comparer;
    }

    public int Count => _nodes.Count;

    public void Enqueue(TElement element, TPriority priority)
    {
        // https://en.wikipedia.org/wiki/Binary_heap#Insert

        // 1.Add the element to the bottom level of the heap at the leftmost open space.
        _nodes.Add((element, priority));

        var nodeIndex = LastIndex;
        while (nodeIndex > 0)
        {
            // 2.Compare the added element with its parent; if they are in the correct order, stop.
            var parentIndex = GetParentIndex(nodeIndex);
            if (_comparer.Compare(_nodes[nodeIndex].Priority, _nodes[parentIndex].Priority) >= 0)
            {
                break;
            }

            // 3.If not, swap the element with its parent and return to the previous step.
            Swap(_nodes, nodeIndex, parentIndex);
            nodeIndex = parentIndex;
        }
    }

    public TElement? Peek()
    {
        return (TryPeek(out var element, out _))
            ? element
            : default;
    }

    public TElement? Dequeue()
    {
        return (TryDequeue(out var element, out _))
            ? element
            : default;
    }

    public bool TryPeek(out TElement? element, out TPriority? priority)
    {
        if (LastIndex < 0)
        {
            element = default;
            priority = default;
            return false;
        }

        (element, priority) = _nodes[0];
        return true;

    }

    public bool TryDequeue(out TElement? element, out TPriority? priority)
    {
        if (LastIndex < 0)
        {
            element = default;
            priority = default;
            return false;
        }

        (element, priority) = _nodes[RootIndex];

        // https://en.wikipedia.org/wiki/Binary_heap#Extract
        // 1. Replace the root of the heap with the last element on the last level.
        Swap(_nodes, RootIndex, LastIndex);
        _nodes.RemoveAt(LastIndex);

        var parentIndex = RootIndex;
        while (parentIndex <= LastIndex)
        {
            // 2. Compare the new root with its children; if they are in the correct order, stop.
            var (leftChildIndex, rightChildIndex) = GetChildrenIndexes(parentIndex);
            if (leftChildIndex > LastIndex)
            {
                break;
            }

            int selectedIndex = leftChildIndex;
            if (rightChildIndex <= LastIndex)
            {
                if (_comparer.Compare(_nodes[leftChildIndex].Priority, _nodes[rightChildIndex].Priority) >= 0)
                {
                    selectedIndex = rightChildIndex;
                }
            }

            if (_comparer.Compare(_nodes[selectedIndex].Priority, _nodes[parentIndex].Priority) >= 0)
            {
                break;
            }

            // 3. If not, swap the element with one of its children and return to the previous step. (Swap with its smaller child in a min-heap and its larger child in a max-heap.)
            Swap(_nodes, selectedIndex, parentIndex);
            parentIndex = selectedIndex;
        }

        return true;
    }

    private static int RootIndex => 0;

    private int LastIndex => _nodes.Count - 1;

    private static int GetParentIndex(int childIndex) => (childIndex - 1) / 2;

    private static (int a, int b) GetChildrenIndexes(int parentIndex)
    {
        var a = parentIndex * 2 + 1;
        var b = parentIndex * 2 + 2;
        return (a, b);
    }

    private static void Swap(IList<(TElement, TPriority)> nodes, int a, int b)
    {
        (nodes[a], nodes[b]) = (nodes[b], nodes[a]);
    }

}
