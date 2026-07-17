using Examples.Design.Anderson.Repositories;

namespace Examples.Design.Anderson.Infrastructures
{
    public static class Factories
    {
        public static ISensorRepository CreateSensorRepository()
        {
            return new SensorFake();
        }

        public static IMeasureRepository CreateMeasureRepository()
        {
            return new MeasureFake();
        }
    }

}
