using System.Runtime.InteropServices;
using System.Security;

namespace Examples.Management;

/// <summary>
/// Represents a wrapper class for a unmanaged memory handle.
/// </summary>
/// <seealso href="https://stackoverflow.com/questions/17562295/if-i-allocate-some-memory-with-allochglobal-do-i-have-to-free-it-with-freehglob/17562399" />
public sealed class SafeUnmanagedMemoryHandle : SafeHandle
{
    private SafeUnmanagedMemoryHandle(IntPtr invalidHandleValue, bool ownsHandle)
        : base(invalidHandleValue, ownsHandle)
    {
    }

    public override bool IsInvalid
        => (handle == IntPtr.Zero) || (handle == new IntPtr(-1));


    [SecurityCritical]
    protected override bool ReleaseHandle()
    {
        if (IsInvalid)
        {
            return false;
        }

        // CER(Constrained Execution Regions) is only supported in .NET Framework.
        // This article doesn't apply to .NET Core or .NET 5 and above.
        // System.Runtime.CompilerServices.RuntimeHelpers.PrepareConstrainedRegions();
        try { }
        finally
        {
            SafeUnmanagedMemoryHandle.Free(handle);
            handle = IntPtr.Zero;
        }

        return true;
    }

    public static IntPtr Allocate(int size)
        => Marshal.AllocCoTaskMem(size);

    public static void Free(IntPtr handle)
        => Marshal.FreeCoTaskMem(handle);

    /// <summary>
    /// A static factory method to create a <see cref="SafeUnmanagedMemoryHandle" />.
    /// </summary>
    /// <param name="size">The required number of bytes in memory.</param>
    /// <returns>A <see cref="SafeUnmanagedMemoryHandle" /> object that wraps unmanaged memory.</returns>
    public static SafeUnmanagedMemoryHandle Create(int size)
    {
        var ptr = SafeUnmanagedMemoryHandle.Allocate(size);
        var handle = new SafeUnmanagedMemoryHandle(ptr, true);

        return handle;
    }

    /// <summary>
    /// A static factory method to create a <see cref="SafeUnmanagedMemoryHandle" />.
    /// </summary>
    /// <param name="structure"> A managed object that holds the data to be marshaled.
    ///     This object must be a structure or an instance of a formatted class.</param>
    /// <typeparam name="TStructure">Type of data to reserve in unmanaged memory.</typeparam>
    /// <returns>A <see cref="SafeUnmanagedMemoryHandle" /> object that wraps unmanaged memory.</returns>
    public static SafeUnmanagedMemoryHandle Create<TStructure>(TStructure structure)
        where TStructure : struct
    {
        var ptr = SafeUnmanagedMemoryHandle.Allocate(Marshal.SizeOf<TStructure>());
        Marshal.StructureToPtr(structure, ptr, false);
        var handle = new SafeUnmanagedMemoryHandle(ptr, true);

        return handle;
    }

}
