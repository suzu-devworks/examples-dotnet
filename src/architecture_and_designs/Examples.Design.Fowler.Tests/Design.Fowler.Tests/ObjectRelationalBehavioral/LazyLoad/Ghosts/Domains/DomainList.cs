using System.Collections;

namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.Domains;

/// <summary>
/// A generic list implementation that acts as a ghost list.
/// </summary>
/// <remarks>
/// <para>Designed to be loaded only once to avoid ripple loading.</para>
/// </remarks>
public class DomainList<T> : DomainObject, IList<T>
{
    public DomainList() : base(0L)
    {
    }

    public IList<T> Data
    {
        get
        {
            Load();
            return _data;
        }
        set
        {
            _data = value;
        }
    }
    private IList<T> _data = new List<T>();

    private new void Load()
    {
        if (IsGhost)
        {
            if (RunLoader is null)
                throw new InvalidOperationException("RunLoader is null.");

            MarkLoading();
            RunLoader(this);
            MarkLoaded();
        }
    }

    public int Count => Data.Count;

    public bool IsReadOnly => Data.IsReadOnly;

    public T this[int index]
    {
        get => Data[index];
        set => Data[index] = value;
    }

    public int IndexOf(T item)
    {
        return Data.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        Data.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        Data.RemoveAt(index);
    }

    public void Add(T item)
    {
        Data.Add(item);
    }

    public void Clear()
    {
        Data.Clear();
    }

    public bool Contains(T item)
    {
        return Data.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        Data.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return Data.Remove(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return Data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Data.GetEnumerator();
    }

    public delegate void Loader(DomainList<T> list);
    public Loader? RunLoader;

}
