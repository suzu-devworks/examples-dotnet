using ChainingAssertion;
using Examples.DDD.Anderson.Repositories;
using Examples.DDD.Anderson.ViewModels;
using Moq;
using Xunit;

#pragma warning disable IDE0051

namespace Examples.DDD.Anderson
{
    public class MeasureViewModelTests
    {
        [Fact]
        void TestMesureSenario()
        {
            var sensorMock = new Mock<ISensorRepository>();
            var viewModel = new MeasureViewModel(sensorMock.Object);
            viewModel.MeasureValue.Is("--");

            sensorMock.Setup(x => x.GetData()).Returns(1.23456f);
            viewModel.Measure();
            viewModel.MeasureValue.Is("1.23 m/s");

            sensorMock.Setup(x => x.GetData()).Returns(2.2f);
            viewModel.Measure();
            viewModel.MeasureValue.Is("2.2 m/s");
            return;
        }

    }
}
