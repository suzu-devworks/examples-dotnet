using System.Collections.Generic;
using Examples.Design.Anderson.Entities;

namespace Examples.Design.Anderson.Repositories
{
    public interface IMeasureRepository
    {
        MeasureEntity GetLatest();

        IReadOnlyList<MeasureEntity> GetData();

        void Save(MeasureEntity entity);

    }
}
