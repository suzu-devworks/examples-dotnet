namespace Examples.Collections.Compatibility.Tests;

/// <summary>
/// Tests for <see cref="EnumerableExtensions" /> methods.
/// </summary>
public class EnumerableExtensionsTests
{
    public class ChunkMethod
    {
        [Theory]
        [InlineData(7, 3)]
        [InlineData(13, 2)]
        [InlineData(23, 1)]
        public void When_SequentialDataBySpecifiedCount_Then_SplitsAsExpected(int count, int expectedChunks)
        {
            // Arrange.
            var source = Enumerable.Range(1, 20);

            // Act.
            var chunks = source.Chunk(count).ToArray();

            // Assert.
            Assert.Equal(expectedChunks, chunks.Length);

            for (var i = 0; i < chunks.Length; i++)
            {
                var chunk = chunks[i];
                if (i < chunks.Length - 1)
                {
                    // Split by a specified number of items
                    Assert.Equal(count, chunk.Count());
                }
                else
                {
                    // Remainder
                    Assert.True(chunk.Count() <= count);
                }
            }
        }

        [Fact]
        public void When_EmptySource_Then_ReturnsEmpty()
        {
            // Arrange.
            var source = Enumerable.Empty<int>();

            // Act.
            var chunks = source.Chunk(5).ToArray();

            // Assert.
            Assert.Empty(chunks);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void When_ChunkSizeIsNegativeOrZero_Then_ThrowsArgumentOutOfRangeException(int count)
        {
            // Arrange.
            var source = Enumerable.Range(1, 10);

            // Act & Assert.
            Assert.Throws<ArgumentOutOfRangeException>(() => source.Chunk(count).ToArray());
        }

        [Fact]
        public void When_NullSource_Then_ThrowsArgumentNullException()
        {
            // Arrange.
            IEnumerable<int> source = null!;

            // Act & Assert.
            Assert.Throws<ArgumentNullException>(() => source.Chunk(1).ToArray());
        }

        [Fact]
        public void When_ComparisonWithOfficialVersion_Then_ReturnsMatch()
        {
            // Arrange.
            var source = Enumerable.Range(1, 20);

            // Act.
            var official = global::System.Linq.Enumerable.Chunk(source, 8).ToArray();
            var chunks = source.Chunk(8).ToArray();

            // Assert.
            Assert.Equal(official.Length, chunks.Length);
            for (var i = 0; i < chunks.Length; i++)
            {
                Assert.Equal(official[i], chunks[i]);
            }
        }
    }

    public class DistinctByMethod
    {
        [Fact]
        public void When_CallingDistinctBy_IsCompatible()
        {
            // Arrange.
            var source = new[] {
                ( A: 100, B: "ABC", C: "1st element." ),
                ( A: 200, B: "ABC", C: "2nd element." ),
                ( A: 300, B: "XYZ", C: "3rd element." ),
                ( A: 400, B: "XYZ", C: "4th element." ),
            };

            // Act.
            var unique = source.DistinctBy(x => x.B).ToArray();

            // Assert.
            Assert.Equal(2, unique.Length);
            Assert.True(unique[0] == source[0]);
            Assert.True(unique[1] == source[2]);
        }

        [Fact]
        public void When_EmptySource_Then_ReturnsEmpty()
        {
            // Arrange.
            var source = Enumerable.Empty<(int A, string B, string C)>();

            // Act.
            var unique = source.DistinctBy(x => x.B).ToArray();

            // Assert.
            Assert.Empty(unique);
        }

        [Fact]
        public void When_NullSource_Then_ThrowsArgumentNullException()
        {
            IEnumerable<int> source = null!;
            Assert.Throws<ArgumentNullException>(() => source.DistinctBy(x => x).ToArray());
        }

        [Fact]
        public void When_NullKeySelector_Then_ThrowsArgumentNullException()
        {
            var source = new[] { 1, 2, 3 };
            Func<int, int> keySelector = null!;
            Assert.Throws<ArgumentNullException>(() => source.DistinctBy(keySelector).ToArray());
        }

        [Fact]
        public void When_ComparisonWithOfficialVersion_Then_ReturnsMatch()
        {
            // Arrange.
            var source = new[] {
                ( A: 100, B: "ABC", C: "1st element." ),
                ( A: 200, B: "ABC", C: "2nd element." ),
                ( A: 300, B: "XYZ", C: "3rd element." ),
                ( A: 400, B: "XYZ", C: "4th element." ),
            };

            // Act.
            var official = global::System.Linq.Enumerable.DistinctBy(source, x => x.B).ToArray();
            var unique = source.DistinctBy(x => x.B).ToArray();

            // Assert.
            Assert.Equal(official.Length, unique.Length);
            for (var i = 0; i < unique.Length; i++)
            {
                Assert.Equal(official[i], unique[i]);
            }
        }
    }
}
