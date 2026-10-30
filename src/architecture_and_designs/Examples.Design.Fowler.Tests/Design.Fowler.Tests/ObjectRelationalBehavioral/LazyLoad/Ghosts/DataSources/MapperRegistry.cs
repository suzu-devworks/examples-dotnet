namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.DataSources;

using Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.Domains;

public class MapperRegistry : DataSource.IDataSource
{
    private static readonly MapperRegistry Singleton = new();
    public static MapperRegistry Instance => Singleton;

    private Dictionary<Type, Mapper> _mappers = new();

    public void Load(DomainObject obj)
    {
        Mapper(obj.GetType()).Load(obj);
    }

    public static void Register<T>(Mapper mapper)
    {
        Instance._mappers.TryAdd(typeof(T), mapper);
    }

    public static void Unregister<T>()
    {
        Instance._mappers.Remove(typeof(T));
    }

    public static Mapper Mapper(Type type)
    {
        return Instance._mappers[type];
    }

    public static Mapper Mapper<T>()
        where T : DomainObject
    {
        return Mapper(typeof(T));
    }

}
