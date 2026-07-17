using System.ComponentModel;
using Examples.Design.Anderson.Infrastructures;
using Examples.Design.Anderson.Repositories;

namespace Examples.Design.Anderson.ViewModels
{
    public class MeasureListViewModel : ViewModelBase
    {
        public MeasureListViewModel()
            : this(Factories.CreateMeasureRepository())
        {
        }

        public MeasureListViewModel(IMeasureRepository measureRepository)
        {
            _measureRepository = measureRepository;
            foreach (var entity in _measureRepository.GetData())
            {
                Measures.Add(new MeasureListViewModelMeasure(entity));
            }
        }

        private readonly IMeasureRepository _measureRepository;

        public BindingList<MeasureListViewModelMeasure> Measures { get; } = new();

    }
}
