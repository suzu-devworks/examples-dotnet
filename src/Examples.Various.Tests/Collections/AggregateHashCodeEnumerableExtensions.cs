namespace Examples.Collections;

/// <summary>
/// Extension methods for  get the hash code that aggregates the object fields.
/// </summary>
public static class AggregateHashCodeEnumerableExtensions
{

    public static int AggregateHashCodeWithEffectiveJava(this IEnumerable<object?> fields)
    {
        unchecked // Overflow is fine, just wrap
        {
            // Select any prime number
            var seed1 = 17;
            var seed2 = 23;

            var hash = fields.Where(x => x is not null)
                        .Aggregate(seed1,
                            (hash, field) => (hash * seed2) + field?.GetHashCode() ?? 0);

            return hash;
        }
    }

    public static int AggregateHashCodeWithELF(this IEnumerable<object?> fields)
    {
        unchecked // Overflow is fine, just wrap
        {
            var seed1 = 0u;

            var hash = fields.Where(x => x is not null)
                        .Aggregate(seed1,
                            (hash, field) =>
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

    public static int AggregateHashCodeWithFNV(this IEnumerable<object?> fields)
    {
        unchecked // Overflow is fine, just wrap
        {
            var seed1 = 2166136261u;
            var seed2 = 16777619u;

            var hash = fields.Where(x => x is not null)
                        .Aggregate(seed1,
                            (hash, field) => (hash * seed2) ^ (uint)(field?.GetHashCode() ?? 0));

            return (int)hash;
        }
    }

    public static int AggregateHashCode(this IEnumerable<object?> fields)
    {
        var hash = new HashCode();

        foreach (var field in fields)
        {
            hash.Add(field?.GetHashCode() ?? 0);
        }

        return hash.ToHashCode();
    }

}
