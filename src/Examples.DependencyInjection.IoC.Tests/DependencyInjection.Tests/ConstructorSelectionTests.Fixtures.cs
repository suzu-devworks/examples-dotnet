using Examples.DependencyInjection.IoC.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.DependencyInjection.Tests;

public partial class ConstructorSelectionTests
{

    private class ClassWithMultipleConstructors
    {
        private readonly IEnumerable<IVerifier> _verifiers;

        public ClassWithMultipleConstructors()
            : this(Enumerable.Empty<IVerifier>())
        {
        }

        [ActivatorUtilitiesConstructor]
        public ClassWithMultipleConstructors(IVerifier verifier)
            : this(new[] { verifier })
        {
        }

        public ClassWithMultipleConstructors(IEnumerable<IVerifier> verifiers)
        {
            _verifiers = verifiers;
        }

        public void Verify()
        {
            foreach (var verifier in _verifiers)
            {
                verifier.Called("Hello DI world!!!");
            }
        }

    }

}
