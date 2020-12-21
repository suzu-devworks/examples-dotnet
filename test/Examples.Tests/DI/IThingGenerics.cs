namespace Examples.DI
{
    interface IThing<T>
    {
        string Name { get; }

        T Default { get; }
    }
}
