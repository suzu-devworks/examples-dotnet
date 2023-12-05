using Examples.DependencyInjection.IoC.Tests;

namespace Examples.DependencyInjection.Tests;

public partial class GenericClassInjectionTests
{

    public interface IProvider<T>
    {
        IThing<T, IEnumerable<T>> GetThing();
    }

    public interface IThing<T1, T2>
    {
        string Name { get; }
        T1? Value { get; }
        void Exec(T2 element);
    }


    private class GenericProvider<T> : IProvider<T>
    {
        private readonly IVerifier _verifier;
        private readonly IThing<T, IEnumerable<T>> _thing;

        public GenericProvider(IVerifier verifier, IThing<T, IEnumerable<T>> thing)
        {
            _verifier = verifier;
            _thing = thing;
        }

        public IThing<T, IEnumerable<T>> GetThing()
        {
            _verifier.Called($"Get {typeof(T).Name}.");
            return _thing;
        }

    }

    private class GenericThing<T1, T2> : IThing<T1, T2>
    {
        private readonly IVerifier _verifier;

        public GenericThing(IVerifier verifier)
        {
            _verifier = verifier;
        }

        public string Name => $"Name of {typeof(T1).Name},{typeof(T2).Name}.";

        public T1? Value => default;

        public void Exec(T2 element)
        {
            _verifier.Called(Name);
            _verifier.Called($"Value is {Value}.");
            return;
        }
    }

}
