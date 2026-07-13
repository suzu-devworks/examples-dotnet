using System;

namespace Examples.Design.Anderson.ValueObjects
{
    public sealed class MeasureDate : ValueObject<MeasureDate>
    {

        public MeasureDate(DateTime value)
            => Value = value;

        public DateTime Value { get; }

        public string DisplayValue
            => Value.ToString("yyyy/MM/dd HH:mm:ss");

        protected override bool EqualsCore(MeasureDate? other)
            => (this.Value == other?.Value);

        public override int GetHashCode()
            => this.Value.GetHashCode();

    }
}
