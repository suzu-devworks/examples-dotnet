namespace Examples.Management.Tests;

/// <summary>
/// Tests <see cref="SafeUnmanagedMemoryHandle" /> class.
/// </summary>
public class SafeUnmanagedMemoryHandleTests
{

    [Fact]
    public void WhenCallingCreate_WithSize()
    {
        SafeUnmanagedMemoryHandle handle;
        using (handle = SafeUnmanagedMemoryHandle.Create(40))
        {
            // Enable handle.
            handle.IsInvalid.IsFalse();
        }
        // Free handle.
        handle.IsInvalid.IsTrue();

        return;
    }

    [Fact]
    public void WhenCallingCreate_WithStructureInstance()
    {
        var tester = new StructureTester { Id = 0 };

        SafeUnmanagedMemoryHandle handle;
        using (handle = SafeUnmanagedMemoryHandle.Create(tester))
        {
            // Enable handle.
            handle.IsInvalid.IsFalse();
        }
        // Free handle.
        handle.IsInvalid.IsTrue();

        return;
    }


    private struct StructureTester
    {
        public int Id;

    }

}
