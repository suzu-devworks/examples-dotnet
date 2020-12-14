using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Moq;
using Xunit;

namespace Examples.Core
{
    public class DisposableWithSafeHandleTest
    {

        public interface IVerifyer 
        {
            void Call(bool disposing);
        }
        
        private class DelivedDisposable : DisposableObject

        {
            private string Name;

            private IVerifyer Verifyer;

            private bool disposed;

            private SafeHandle MySafeHandle  = new SafeFileHandle(IntPtr.Zero, true);

            public DelivedDisposable(string name, IVerifyer verifyer) 
                => (Name, Verifyer) = (name, verifyer);

            //~DelivedDisposable() => Dispose(false);

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                
                if (disposed)
                {
                    return;
                }

                if (disposing) 
                {
                    MySafeHandle?.Dispose();
                }

                Verifyer?.Call(disposing);
                Console.WriteLine($"Called Dispose({disposing}) in {Name}");

                disposed = true;

            }
        }

        [Fact]
        void TestUsingDisposed()
        {
            var mock = new Mock<IVerifyer>();
            mock.Setup(x => x.Call(true));
            mock.Setup(x => x.Call(false));

            using (var obj = new DelivedDisposable("Use using.", mock.Object))
            {
            }

            GC.Collect();
            Console.WriteLine($"Called GC.Collect in {nameof(TestUsingDisposed)}");
            GC.WaitForPendingFinalizers();

            mock.Verify(x => x.Call(true), Times.Once());
            mock.Verify(x => x.Call(false), Times.Never());
            mock.VerifyNoOtherCalls();
        }

        [Fact]
        void TestDestructorDisposed()
        {
            var mock = new Mock<IVerifyer>();
            mock.Setup(x => x.Call(true));
            mock.Setup(x => x.Call(false));

            Action action = () => { var obj = new DelivedDisposable("Use Action.", mock.Object); };
            action();

            GC.Collect();
            Console.WriteLine($"Called GC.Collect in {nameof(TestDestructorDisposed)}");
            GC.WaitForPendingFinalizers();

            mock.Verify(x => x.Call(true), Times.Never());
            mock.Verify(x => x.Call(false), Times.Once());
            mock.VerifyNoOtherCalls();
       } 

    }
}