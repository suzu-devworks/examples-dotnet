namespace Examples.Collections.Compatibility.Tests;

/// <summary>
/// Tests for <see cref="EnumerableExtensions" /> methods.
/// </summary>
public class EnumerableExtensionsTests
{

    [Fact]
    public void WhenCallingChunk_IsCompatible()
    {
        var source = Enumerable.Range(1, 20);

        // do.
        var original = global::System.Linq.Enumerable.Chunk(source, 8).ToArray();
        var chunks = source.Chunk(8).ToArray();

        // assert.
        chunks.Length.Is(3);
        chunks[0].Is([1, 2, 3, 4, 5, 6, 7, 8]);
        chunks[1].Is([9, 10, 11, 12, 13, 14, 15, 16]);
        chunks[2].Is([17, 18, 19, 20]);

        chunks.Length.Is(original.Length);
        chunks[0].Is(original[0]);
        chunks[1].Is(original[1]);
        chunks[2].Is(original[2]);

        return;
    }

    [Fact]
    public void WhenCallingDistinctBy_IsCompatible()
    {
        var source = new[] {
            ( A: 100, B: "ABC", C: "1st element." ),
            ( A: 200, B: "ABC", C: "2nd element." ),
            ( A: 300, B: "XYZ", C: "3rd element." ),
            ( A: 400, B: "XYZ", C: "4th element." ),
        };

        // do.
        var original = Enumerable.DistinctBy(source, x => x.B).ToArray();
        var unique = source.DistinctBy(x => x.B).ToArray();

        // assert.
        unique.Length.Is(2);
        (unique[0] == source[0]).IsTrue();
        (unique[1] == source[2]).IsTrue();

        unique.Length.Is(original.Length);
        unique[0].Is(original[0]);
        unique[1].Is(original[1]);

        return;
    }


}
