using Examples.Design.Anderson.ValueObjects;

namespace Examples.Design.Anderson.Repositories
{
    public interface ISensorRepository
    {
        MeasureValue GetData();
    }

}
