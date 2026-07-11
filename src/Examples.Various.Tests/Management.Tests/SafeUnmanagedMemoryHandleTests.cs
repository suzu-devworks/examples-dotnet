namespace Examples.Management.Tests;

/// <summary>
/// Tests <see cref="SafeUnmanagedMemoryHandle" /> class.
/// </summary>
public class SafeUnmanagedMemoryHandleTests
{
    private struct StructureTester
    {
        public int Id;
    }

    [Fact]
    public void When_MemorySizeIsSpecified_Then_CanAllocateUnmanagedMemoryThatValidWithinScope()
    {
        SafeUnmanagedMemoryHandle handle;
        using (handle = SafeUnmanagedMemoryHandle.Create(40))
        {
            // Enable handle.
            Assert.False(handle.IsInvalid);
        }

        // Free handle.
        Assert.True(handle.IsInvalid);
    }

    [Fact]
    public void When_CStructureInstanceSpecified_Then_MarshalingStructureToUnmanagedMemory()
    {
        var tester = new StructureTester { Id = 0 };

        SafeUnmanagedMemoryHandle handle;
        using (handle = SafeUnmanagedMemoryHandle.Create(tester))
        {
            // Enable handle.
            Assert.False(handle.IsInvalid);
        }

        // Free handle.
        Assert.True(handle.IsInvalid);
    }
}
