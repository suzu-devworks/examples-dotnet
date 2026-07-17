using System;
using Examples.Design.Anderson.Helpers;
using Examples.Design.Anderson.Repositories;
using Examples.Design.Anderson.ValueObjects;

namespace Examples.Design.Anderson.Infrastructures
{
    internal sealed class SensorFake : ISensorRepository
    {
        private readonly Random _random = new();

        public MeasureValue GetData()
        {
            return new MeasureValue(_random.Next(0, 3) + _random.NextDouble().ToSingle());
        }
    }
}
