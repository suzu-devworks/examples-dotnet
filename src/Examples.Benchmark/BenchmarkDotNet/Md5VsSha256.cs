using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;

namespace Examples.Benchmark.BenchmarkDotNet;

// [DryJob]
// [SimpleJob(RuntimeMoniker.Net472, baseline: true)]
// [SimpleJob(RuntimeMoniker.NetCoreApp30)]
// [SimpleJob(RuntimeMoniker.NativeAot70)]
// [SimpleJob(RuntimeMoniker.Mono)]
// [RPlotExporter]
[SimpleJob(RuntimeMoniker.Net80)]
// == GC and Memory Allocation ==
[MemoryDiagnoser]
// == Code size and disassembly ==
[DisassemblyDiagnoser]
// Threading statistics:
[ThreadingDiagnoser]
[MinColumn, MaxColumn, MedianColumn, SkewnessColumn, KurtosisColumn]
public class Md5VsSha256
{
    private readonly SHA256 _sha256 = SHA256.Create();
    private readonly MD5 _md5 = MD5.Create();
    private byte[] _data = default!;

    [Params(1000, 10000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        _data = new byte[N];
        new Random(42).NextBytes(_data);
    }

    [Benchmark]
    public byte[] Sha256() => _sha256.ComputeHash(_data);

    [Benchmark]
    public byte[] Md5() => _md5.ComputeHash(_data);
}
