using System.Data;
using Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.Domains;

namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.DataSources;

public class ListLoader
{
    public string? Sql { get; set; }
    public IDictionary<string, object> SqlParams { get; set; }
        = new Dictionary<string, object>();
    public Mapper? Mapper { get; set; }

    public void Attach<T>(DomainList<T> list)
        where T : DomainObject
    {
        list.RunLoader = new DomainList<T>.Loader(Load<T>);
    }

    private void Load<T>(DomainList<T> list)
        where T : DomainObject
    {
        ArgumentException.ThrowIfNullOrEmpty(Sql);
        if (Mapper is null) throw new InvalidOperationException("Mapper is null");

        using IDbCommand comm = Mapper.Connection.CreateCommand();
        comm.CommandText = Sql;
        foreach (var (key, value) in SqlParams)
        {
            var param = comm.CreateParameter();
            param.ParameterName = key;
            param.Value = value;
            comm.Parameters.Add(param);
        }

        using IDataReader reader = comm.ExecuteReader();
        while (reader.Read())
        {
            DomainObject obj = GhostForLine(reader);
            Mapper?.LoadLine(reader, obj);
            list.Add((T)obj);
        }
        reader.Close();
    }

    private DomainObject GhostForLine(IDataReader reader)
    {
        if (Mapper is null) throw new InvalidOperationException("Mapper is null");

        return Mapper.AbstractFind((long)reader[Mapper.KeyColumnName]);
    }
}
