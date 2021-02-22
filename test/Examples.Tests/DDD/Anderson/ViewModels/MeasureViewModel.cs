using System;
using Examples.DDD.Anderson.Repositories;

namespace Examples.DDD.Anderson.ViewModels
{
    public class MeasureViewModel : ViewModelBase
    {
        public MeasureViewModel(ISensorRepository sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }

        private readonly ISensorRepository _sensorRepository;

        private string _measureValue = "--";
        public string MeasureValue
        {
            get { return _measureValue; }
            set { SetProperty(ref _measureValue, value); }
        }

        public void Measure()
        {
            var value = _sensorRepository.GetData();
            MeasureValue = Math.Round(value, 2) + " m/s";
        }

    }
}
