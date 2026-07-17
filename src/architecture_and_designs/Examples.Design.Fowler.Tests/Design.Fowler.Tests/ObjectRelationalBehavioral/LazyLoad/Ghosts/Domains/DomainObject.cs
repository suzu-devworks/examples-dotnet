using System.Diagnostics;

namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.Domains;

/// <summary>
/// Layer supertype of domain objects.
/// </summary>
/// <remarks>
/// <para>Put the object ghosting logic into the layer supertype.</para>
/// </remarks>
public abstract class DomainObject
{
    private LoadStatus _status;

    public DomainObject(long key)
    {
        this.Key = key;
    }

    public long Key { get; }

    public bool IsGhost => _status == LoadStatus.Ghosts;

    public bool IsLoaded => _status == LoadStatus.Loaded;

    public void MarkLoading()
    {
        Debug.Assert(IsGhost);
        _status = LoadStatus.Loading;
    }

    public void MarkLoaded()
    {
        Debug.Assert(_status == LoadStatus.Loading);
        _status = LoadStatus.Loaded;
    }

    protected void Load()
    {
        if (IsGhost) DataSource.Load(this);
    }

    public enum LoadStatus
    {
        Ghosts,
        Loading,
        Loaded,
    }

}
