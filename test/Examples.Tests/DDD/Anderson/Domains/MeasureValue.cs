namespace Examples.DDD.Anderson.Domains
{
    public sealed class MeasureValue : ValueObject<MeasureValue>
    {

        public MeasureValue(double value)
            => Value = value;

        public double Value { get; }

        protected override bool EqualsCore(MeasureValue other)
            => (this.Value == other.Value);

        public override int GetHashCode()
            => this.Value.GetHashCode();

    }
}
