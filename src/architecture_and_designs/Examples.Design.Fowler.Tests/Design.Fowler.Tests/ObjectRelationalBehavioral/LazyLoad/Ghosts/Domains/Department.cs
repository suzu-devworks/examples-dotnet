namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.Domains;

/// <summary>
/// Department Domain Object.
/// </summary>
/// <remarks>
/// <para>Omit all but the necessary parts.</para>
/// </remarks>
public class Department : DomainObject
{
    public Department(long key) : base(key)
    {
    }

    public string Name
    {
        get
        {
            Load();
            return _name!;
        }
        set
        {
            Load();
            _name = value;
        }
    }
    private string? _name;

}
