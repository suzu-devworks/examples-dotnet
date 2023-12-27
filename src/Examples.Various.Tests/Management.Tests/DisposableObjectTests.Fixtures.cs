namespace Examples.Management.Tests;

public partial class DisposableObjectTests
{

    public interface IVerifier
    {
        void Called(string message);

    }

    private class DisposableTester : DisposableObject
    {
        private readonly IVerifier _verifier;

        public DisposableTester(IVerifier verifier)
        {
            _verifier = verifier;
        }

        protected override void DisposeManagedState()
        {
            _verifier.Called($"DisposeManagedState");
            base.DisposeManagedState();
        }

        protected override void FreeUnmanagedResources(bool disposing)
        {
            _verifier.Called($"FreeUnmanagedResources({disposing})");
            base.FreeUnmanagedResources(disposing);
        }


        protected override ValueTask DisposeAsyncCore()
        {
            _verifier.Called("DisposeAsync");
            return base.DisposeAsyncCore();
        }

    }

}
