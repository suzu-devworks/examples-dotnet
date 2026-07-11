namespace Examples.Graphics.Tests;

public class ImageExtensionsTests
{
    public sealed class GetMimeTypeMethod
    {
        [Theory]
        [InlineData(new byte[] { 0xff, 0xd8 }, "image/jpeg")]
        [InlineData(new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a }, "image/png")]
        [InlineData(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }, "image/gif")]
        [InlineData(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, "image/gif")]
        [InlineData(new byte[] { 0x52, 0x49, 0x46, 0x46 }, "image/webp")]
        [InlineData(new byte[] { 0x42, 0x4d }, "image/bmp")]
        [InlineData(new byte[] { 0x00, 0x01, 0x02, 0x03 }, "application/octet-stream")]
        [InlineData(new byte[] { }, "application/octet-stream")]
        public void When_BytesProvided_Then_ReturnsMimeType(byte[] bytes, string expectedMimeType)
        {
            // Act
            string result = bytes.GetMimeType();

            // Assert
            Assert.Equal(expectedMimeType, result);
        }

        [Theory]
        [InlineData(new byte[] { 0xff, 0xd8, 0xff, 0xe0, 0x00, 0x10, 0x4a, 0x46 }, "image/jpeg")]
        [InlineData(new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a, 0x00, 0x00 }, "image/png")]
        public void When_StreamProvided_Then_ReturnsMimeType(byte[] bytes, string expectedMimeType)
        {
            // Arrange
            using var stream = new MemoryStream(bytes);

            // Act
            string result = stream.GetMimeType();

            // Assert
            Assert.Equal(expectedMimeType, result);
        }

        [Theory]
        [InlineData(new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a, 0x00, 0x00 }, 8)]
        public void When_StreamRead_Then_PositionIsUpdated(byte[] bytes, int expectedPosition)
        {
            // Arrange
            using var stream = new MemoryStream(bytes);

            // Act
            string result = stream.GetMimeType();

            // Assert
            Assert.Equal(expectedPosition, stream.Position);
        }
    }
}
