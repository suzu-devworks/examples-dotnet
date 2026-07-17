using System.Data;
using Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.Domains;

namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.DataSources;

public class TimeRecordMapper : Mapper
{
    public const string FIND_FOR_EMPLOYEE_SQL
        = "SELECT id FROM time_records WHERE employee_id = @employee_key;";

    public TimeRecordMapper(IDbConnection connection) : base(connection)
    {
    }

    protected override DomainObject CreateGhost(long key)
    {
        return new TimeRecord(key);
    }

    protected override void DoLoadLine(IDataReader reader, DomainObject obj)
    {
        TimeRecord timeRecord = (TimeRecord)obj;
        timeRecord.RecordedAt = (DateTime)reader["recorded_at"];
    }

    protected override string FindStatement()
    {
        return "SELECT employee_id FROM time_records WHERE id = @key;";
    }
}
