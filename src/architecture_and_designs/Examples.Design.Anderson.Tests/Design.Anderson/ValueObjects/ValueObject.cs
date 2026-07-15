namespace Examples.Design.Anderson.ValueObjects
{
    public abstract class ValueObject<T>
        where T : ValueObject<T>
    {
        public override bool Equals(object? obj)
        {
            if (obj is not T)
            {
                return false;
            }

            return EqualsCore((T?)obj);
        }

        protected abstract bool EqualsCore(T? other);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(ValueObject<T>? value1, ValueObject<T>? value2)
        {
            return Equals(value1, value2);
        }

        public static bool operator !=(ValueObject<T>? value1, ValueObject<T>? value2)
        {
            return !Equals(value1, value2);
        }

    }
}
