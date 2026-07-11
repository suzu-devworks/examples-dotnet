namespace Examples.Management;

public static class HashCodeCalculator
{
    public static IHashCodeCalculator BasedOnEffectiveJava { get; }
            = new BasedOnEffectiveJavaHashCodeCalculator();

    public static IHashCodeCalculator Elf { get; }
            = new ElfHashCodeCalculator();

    public static IHashCodeCalculator Fnv { get; }
            = new FnvHashCodeCalculator();

    public interface IHashCodeCalculator
    {
        int GetHashCode(params object?[] fields);
    }

    private class BasedOnEffectiveJavaHashCodeCalculator : IHashCodeCalculator
    {
        public int GetHashCode(params object?[] fields)
        {
            unchecked // Overflow is fine, just wrap
            {
                // Select any prime number
                var seed1 = 17;
                var seed2 = 23;

                var hash = fields
                            .Where(field => field is not null)
                            .Aggregate(seed1, (hash, field) =>
                            {
                                return (hash * seed2) + field!.GetHashCode();
                            });

                return hash;
            }
        }
    }

    private class ElfHashCodeCalculator : IHashCodeCalculator
    {
        public int GetHashCode(params object?[] fields)
        {
            unchecked // Overflow is fine, just wrap
            {
                uint seed1 = 0u;
                uint hash = fields
                            .Where(x => x is not null)
                            .Aggregate(seed1, (hash, field) =>
                            {
                                hash = (hash << 4) + (uint)(field?.GetHashCode() ?? 0);
                                var x = hash & 0xF0000000;
                                if (x != 0u)
                                {
                                    hash ^= (x >> 24);
                                }
                                hash &= ~x;
                                return hash;
                            });

                return (int)hash;
            }
        }
    }

    private class FnvHashCodeCalculator : IHashCodeCalculator
    {
        public int GetHashCode(params object?[] fields)
        {
            unchecked // Overflow is fine, just wrap
            {
                uint seed1 = 2166136261u;
                uint seed2 = 16777619u;

                uint hash = fields
                            .Where(x => x is not null)
                            .Aggregate(seed1, (hash, field) =>
                            {
                                return (hash * seed2) ^ (uint)(field?.GetHashCode() ?? 0);
                            });

                return (int)hash;
            }
        }
    }
}
