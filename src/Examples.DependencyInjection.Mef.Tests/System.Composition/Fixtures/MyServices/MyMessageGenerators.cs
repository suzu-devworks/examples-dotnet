namespace Examples.Tests.System.Composition.Fixtures.MyServices;

public class MyMessageGenerator1 : IMessageGenerator
{
    public string Generate() => "Hello MEF2 DI world 1st.";
}

public class MyMessageGenerator2 : IMessageGenerator
{
    public string Generate() => "Hello MEF2 DI world 2nd.";
}

public class MyMessageGenerator3 : IMessageGenerator
{
    public string Generate() => "Hello MEF2 DI world 3rd.";
}
