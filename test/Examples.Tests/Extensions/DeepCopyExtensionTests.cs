using System;
using Xunit;
using ChainingAssertion;

namespace Examples.Extensions
{
    public class DeepCopyExtensionTests
    {

        [Serializable]
        private class FatClass : ICloneable
        {
            //ICloneable is considered a bad API now, since it does not specify whether the result is a deep or a shallow copy. 
            // I think this is why they do not improve this interface.

            public string Code { get; set; }
            public string Name { get; set; }
            public object Clone()
            {
                var obj = this.MemberwiseClone() as FatClass;
                //other reference member copy...
                return obj;
            }
        }

        [Fact]
        void TestUsingDisposed()
        {
            var obj = new FatClass() { Code = "123", Name = "foo" };
            var other0 = obj;
            var other1 = obj.Clone() as FatClass;
            var other2 = obj.DeepCopy();

            other0.Is(obj);

            other1.IsNot(obj);
            other1.Code.Is(obj.Code);
            other1.Name.Is(obj.Name);

            other2.IsNot(obj);
            other2.Code.Is(obj.Code);
            other2.Name.Is(obj.Name);
        }

    }
}