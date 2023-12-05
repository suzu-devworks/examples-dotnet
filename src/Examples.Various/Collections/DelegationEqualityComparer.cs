namespace Examples.Collections;

public class DelegationEqualityComparer<T> : IEqualityComparer<T>
    where T : notnull
{
    private readonly Func<T, T, bool> _comparer;

    public DelegationEqualityComparer(Func<T, T, bool> comparer)
    {
        _comparer = comparer;
    }

    public bool Equals(T? x, T? y)
    {
        if (ReferenceEquals(x, y)) { return true; }
        if ((x is null) || (y is null)) { return false; }

        return _comparer(x, y);
    }

    public int GetHashCode(T obj)
    {
        return obj.GetType().GetHashCode();
    }

}
