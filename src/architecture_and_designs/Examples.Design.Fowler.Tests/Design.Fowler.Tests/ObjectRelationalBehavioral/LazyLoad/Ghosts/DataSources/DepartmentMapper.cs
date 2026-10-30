using System.Data;
using Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.Domains;

namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.DataSources;

public class DepartmentMapper : Mapper
{
    public DepartmentMapper(IDbConnection connection) : base(connection)
    {
    }

    public Department? Find(long key)
    {
        return (Department)AbstractFind(key);
    }

    protected override DomainObject CreateGhost(long key)
    {
        return new Department(key);
    }

    protected override void DoLoadLine(IDataReader reader, DomainObject obj)
    {
        Department department = (Department)obj;
        department.Name = (string)reader["name"];
    }

    protected override string FindStatement()
    {
        return "SELECT name FROM department WHERE id = @key;";
    }
}
