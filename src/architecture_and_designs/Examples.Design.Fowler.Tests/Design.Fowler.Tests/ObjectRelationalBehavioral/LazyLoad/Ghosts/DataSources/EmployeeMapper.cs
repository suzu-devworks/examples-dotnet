using System.Data;
using Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.Domains;

namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.DataSources;

public class EmployeeMapper : Mapper
{
    public EmployeeMapper(IDbConnection connection) : base(connection)
    {
    }

    public Employee? Find(long key)
    {
        return (Employee)AbstractFind(key);
    }

    protected override DomainObject CreateGhost(long key)
    {
        return new Employee(key);
    }

    protected override void DoLoadLine(IDataReader reader, DomainObject obj)
    {
        Employee employee = (Employee)obj;
        employee.Name = (string)reader["name"];

        DepartmentMapper depMapper
            = (DepartmentMapper)MapperRegistry.Mapper<Department>();
        employee.Department = depMapper.Find((long)reader["department_id"]);

        LoadTimeRecords(employee);
    }

    private static void LoadTimeRecords(Employee employee)
    {
        ListLoader loader = new ListLoader();
        loader.Sql = TimeRecordMapper.FIND_FOR_EMPLOYEE_SQL;
        loader.SqlParams.Add("employee_key", employee.Key);
        loader.Mapper = MapperRegistry.Mapper<TimeRecord>();
        loader.Attach(employee.TimeRecords);
    }

    protected override string FindStatement()
    {
        return "SELECT name, department_id FROM employee WHERE id = @key;";
    }
}
