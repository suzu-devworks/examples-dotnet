using System.Runtime.InteropServices;
using Examples.Plugins.Tutorials;

namespace Examples.Plugins.Libuv;

public class UvCommand : ICommand
{
    public string Name => "uv";

    public string Description => "Uses the native library libuv to show its version.";

    public int Execute(TextWriter output)
    {
        output.WriteLine($"Using libuv version {GetVersion()}.");
        return 0;
    }

    [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
    // [LibraryImport("libuv", EntryPoint = "uv_version", StringMarshalling = StringMarshalling.Utf8)]
    private extern static uint uv_version();

    private static Version GetVersion()
    {
        uint version = uv_version();
        return new Version((int)(version & 0xFF0000) >> 16, (int)(version & 0xFF00) >> 8, (int)(version & 0xFF));
    }
}
