using Examples.Design.Anderson.Entities;

namespace Examples.Design.Anderson.ViewModels
{
    public class MeasureListViewModelMeasure
    {

        private readonly MeasureEntity _entity;

        public MeasureListViewModelMeasure(MeasureEntity entity)
        {
            _entity = entity;
        }

        public string MeasureDate
            => _entity.MeasureDate.DisplayValue;

        public string MeasureValue
            => _entity.MeasureValue.DisplayValue;

    }
}
