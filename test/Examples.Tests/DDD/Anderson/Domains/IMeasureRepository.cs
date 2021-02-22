namespace Examples.DDD.Anderson.Domains
{
    public interface IMeasureRepository
    {
        MeasureEntity GetLatest();
    }
}
