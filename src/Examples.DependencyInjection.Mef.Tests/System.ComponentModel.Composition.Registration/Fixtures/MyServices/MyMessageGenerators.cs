// using System.ComponentModel.Composition;

namespace Examples.Tests.System.ComponentModel.Composition.Registration.Fixtures.MyServices;

// [Export(typeof(IMessageGenerator))]
// [PartCreationPolicy(CreationPolicy.Shared)]
public class MyMessageGenerator1 : IMessageGenerator
{
    public string Generate() => "Hello MEF' DI world 1st.";
}

// [Export(typeof(IMessageGenerator))]
// [PartCreationPolicy(CreationPolicy.NonShared)]
public class MyMessageGenerator2 : IMessageGenerator
{
    public string Generate() => "Hello MEF' DI world 2nd.";
}

// [Export(typeof(IMessageGenerator))]
// [PartCreationPolicy(CreationPolicy.Any)]
public class MyMessageGenerator3 : IMessageGenerator
{
    public string Generate() => "Hello MEF' DI world 3rd.";
}
