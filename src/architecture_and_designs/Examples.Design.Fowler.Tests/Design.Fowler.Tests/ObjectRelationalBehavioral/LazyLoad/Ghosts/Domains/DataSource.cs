namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts.Domains;

/// <summary>
/// Datasource Registry for the Domain Layer.
/// </summary>
public class DataSource
{
    public interface IDataSource
    {
        void Load(DomainObject obj);
    }

    private static readonly DataSource Singleton = new();
    public static DataSource Instance => Singleton;

    private IDataSource? _dataSource;

    public static void Init(IDataSource dataSource)
    {
        Instance._dataSource = dataSource;
    }

    public static void Load(DomainObject obj)
    {
        if (Instance._dataSource is null)
            throw new InvalidOperationException("DataSource is null");

        Instance._dataSource.Load(obj);
    }

}
