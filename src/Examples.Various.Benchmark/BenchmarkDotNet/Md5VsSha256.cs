using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;

namespace BenchmarkDotNet;

// [SimpleJob(RuntimeMoniker.Net472, baseline: true)]
// [SimpleJob(RuntimeMoniker.NetCoreApp30)]
// [SimpleJob(RuntimeMoniker.NativeAot70)]
// [SimpleJob(RuntimeMoniker.Mono)]
// [RPlotExporter]
public class Md5VsSha256
{
    private SHA256 _sha256 = SHA256.Create();
    private MD5 _md5 = MD5.Create();
    private byte[] _data = [];

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
