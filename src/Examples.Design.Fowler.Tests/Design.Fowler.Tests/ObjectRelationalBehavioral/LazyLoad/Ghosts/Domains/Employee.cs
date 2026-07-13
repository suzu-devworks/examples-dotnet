namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.Domains;

/// <summary>
/// Employee Domain Object.
/// </summary>
/// <remarks>
/// <para>Domain objects must explicitly implement all access functions that trigger loading.</para>
/// </remarks>
public class Employee : DomainObject
{
    public Employee(long key) : base(key)
    {
    }

    public string? Name
    {
        get
        {
            Load();
            return _name;
        }
        set
        {
            Load();
            _name = value;
        }
    }
    private string? _name;

    public Department? Department
    {
        get
        {
            Load();
            return _department;
        }
        set
        {
            Load();
            _department = value;
        }
    }
    private Department? _department;

    public DomainList<TimeRecord> TimeRecords
    {
        get
        {
            Load();
            return _timeRecords!;
        }
    }
    private DomainList<TimeRecord> _timeRecords = [];

}
