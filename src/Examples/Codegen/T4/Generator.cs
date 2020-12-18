using System;

namespace Examples.Codegen.T4
{
    public class Generator : IRunner
    {
        public void Run()
        {
            GenerateHelloWorld();
            GenerateSampleTemplating();

            return;
        }

        private static void GenerateHelloWorld()
        {
            // Run using "dynamic", but slow.
            dynamic template = Activator.CreateInstance(Type.GetType("Examples.HelloWorld"));
            template.Session = new Microsoft.VisualStudio.TextTemplating.TextTemplatingSession();
            template.Session["Name"] = "T4";
            template.Initialize();
            Console.WriteLine(template.TransformText());

            return;
        }

        private static void GenerateSampleTemplating()
        {
            // Create a default implementation of the TransformText () in Interface.
            var template = new SampleTemplating
            {
                Value = "hoge"
            };
            Console.WriteLine((template as ITemplate)?.TransformText());

            return;
        }


    }
}
