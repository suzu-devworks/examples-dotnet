namespace Examples.Caching.Tests._.Microsoft.Extensions.Caching;

public partial class MemoryCacheTests
{
    public interface IVerifier
    {
        void Called(string message);

    }

    private class Sample
    {
        public int? Number { get; init; }
    }

}
