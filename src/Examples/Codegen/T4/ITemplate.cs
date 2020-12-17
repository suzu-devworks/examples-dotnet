using System;

namespace Examples.Codegen.T4
{
    public interface ITemplate
    {
        string TransformText() => throw new NotImplementedException();
    }
}
