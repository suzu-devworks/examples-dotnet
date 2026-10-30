using System.Data;
using Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.Domains;

namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.DataSources;

public abstract class Mapper
{
    public Mapper(IDbConnection connection)
    {
        Connection = connection;
        KeyColumnName = "id";
    }

    public IDbConnection Connection { get; }

    public DomainObject AbstractFind(long key)
    {
        DomainObject? result = _loadedMap.GetValueOrDefault(key);
        if (result is null)
        {
            result = CreateGhost(key);
            _loadedMap.Add(key, result);
        }
        return result;
    }

    private Dictionary<long, DomainObject> _loadedMap = new();

    protected abstract DomainObject CreateGhost(long key);

    public void Load(DomainObject obj)
    {
        if (!obj.IsGhost) return;

        using IDbCommand comm = Connection.CreateCommand();
        comm.CommandText = FindStatement();

        var param = comm.CreateParameter();
        param.ParameterName = "key";
        param.Value = obj.Key;
        comm.Parameters.Add(param);

        using IDataReader reader = comm.ExecuteReader();
        reader.Read();
        LoadLine(reader, obj);
        reader.Close();
    }

    protected abstract string FindStatement();

    public void LoadLine(IDataReader reader, DomainObject obj)
    {
        if (obj.IsGhost)
        {
            obj.MarkLoading();
            DoLoadLine(reader, obj);
            obj.MarkLoaded();
        }
    }

    protected abstract void DoLoadLine(IDataReader reader, DomainObject obj);

    public virtual string KeyColumnName { get; }

}
