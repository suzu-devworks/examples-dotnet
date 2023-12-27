namespace Examples.Management.Tests;

/// <summary>
/// Tests <see cref="CloneableExtensions" /> methods.
/// </summary>
public class CloneableExtensionsTests
{

    [Fact]
    public void WhenCallingIClonableClone()
    {
        var obj = new FatClass() { Code = "123", Name = "foo" };
        var other = obj.Clone() as FatClass;

        // clone object is another instance.
        (other == obj).IsFalse();
        ReferenceEquals(obj, other).IsFalse();

        // member is shallow copy.
        (other!.Code == obj.Code).IsTrue();
        (other!.Name == obj.Name).IsTrue();
        ReferenceEquals(obj.Code, other.Code).IsTrue();
        ReferenceEquals(obj.Name, other.Name).IsTrue();

        return;
    }

    [Fact]
    public void WhenCallingDeepCopy()
    {
        var obj = new FatClass() { Code = "123", Name = "foo" };
        var other = obj.DeepCopy();

        // clone object is another instance.
        (other == obj).IsFalse();
        ReferenceEquals(obj, other).IsFalse();

        // member is deep copies.
        (other.Code == obj.Code).IsTrue();
        (other.Name == obj.Name).IsTrue();
        ReferenceEquals(obj.Code, other.Code).IsFalse();
        ReferenceEquals(obj.Name, other.Name).IsFalse();

        return;
    }

    [Fact]
    public void WhenCallingShallowCopy()
    {
        var obj = new FatClass() { Code = "123", Name = "foo" };
        var other = obj.ShallowCopy();

        // clone object is another instance.
        (other == obj).IsFalse();
        ReferenceEquals(obj, other).IsFalse();

        // member is shallow copy.
        (other!.Code == obj.Code).IsTrue();
        (other!.Name == obj.Name).IsTrue();
        ReferenceEquals(obj.Code, other.Code).IsTrue();
        ReferenceEquals(obj.Name, other.Name).IsTrue();

        return;
    }

    [Serializable]
    private class FatClass : ICloneable
    {
        //ICloneable is considered a bad API now, since it does not specify whether the result is a deep or a shallow copy.
        // I think this is why they do not improve this interface.

        public string? Code { get; set; }
        public string? Name { get; set; }
        public object Clone()
        {
            var copied = (FatClass)MemberwiseClone();
            //other reference member copy...

            return copied;
        }
    }

}
