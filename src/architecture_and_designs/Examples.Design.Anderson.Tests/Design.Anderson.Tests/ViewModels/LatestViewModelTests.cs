using Examples.Design.Anderson.Entities;
using Examples.Design.Anderson.Helpers;
using Examples.Design.Anderson.Repositories;
using NSubstitute;

namespace Examples.Design.Anderson.ViewModels.Tests;

public class LatestViewModelTests
{
    [Fact]
    public void When_ConstructingLatestViewModel_WithMockRepository_Then_ReturnsExpectedValues()
    {
        var measureMock = Substitute.For<IMeasureRepository>();
        var measure = new MeasureEntity("guidA", "2017/01/01 13:00:00".ToDate(), 1.23456f);
        measureMock.GetLatest().Returns(measure);

        var viewModel = new LatestViewModel(measureMock);
        Assert.Equal("2017/01/01 13:00:00", viewModel.MeasureDate);
        Assert.Equal("1.23m/s", viewModel.MeasureValue);
    }

    [Fact]
    public void When_ConstructingLatestViewModel_WithFakeRepository_Then_ReturnsExpectedValues()
    {
        var viewModel = new LatestViewModel();
        Assert.Equal("2017/01/01 13:00:00", viewModel.MeasureDate);
        Assert.Equal("1.23m/s", viewModel.MeasureValue);
        return;
    }

}
