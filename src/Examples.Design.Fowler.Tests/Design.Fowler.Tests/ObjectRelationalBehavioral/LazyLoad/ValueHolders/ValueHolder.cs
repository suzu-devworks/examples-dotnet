namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.ValueHolders;

// A general-purpose class responsible for lazy loading of values.
public class ValueHolder<T>
{
    private T? _value;
    private readonly Func<T> _loader;
    private bool _isLoaded = false;

    public ValueHolder(Func<T> loader)
    {
        _loader = loader;
    }

    public T Value
    {
        get
        {
            if (!_isLoaded)
            {
                _value = _loader();
                _isLoaded = true;
            }
            return _value!;
        }
    }
}
