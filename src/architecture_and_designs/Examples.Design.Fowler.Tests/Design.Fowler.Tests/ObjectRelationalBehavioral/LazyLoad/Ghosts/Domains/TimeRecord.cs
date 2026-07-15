namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.Domains;

/// <summary>
/// TimeRecord Domain Object.
/// </summary>
/// <remarks>
/// <para>Omit all but the necessary parts.</para>
/// </remarks>
public class TimeRecord : DomainObject
{
    public TimeRecord(long key) : base(key)
    {
    }

    public DateTime RecordedAt
    {
        get
        {
            Load();
            return _recordedAt!.Value;
        }
        set
        {
            Load();
            _recordedAt = value;
        }
    }
    private DateTime? _recordedAt;

}
