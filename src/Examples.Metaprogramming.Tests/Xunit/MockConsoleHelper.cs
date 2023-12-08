using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examples.Xunit;

public class MockConsoleHelper
{
    public static void RunTest(TextWriter writer, Action runnable)
    {
        TextWriter stdout = Console.Out;
        try
        {
            Console.SetOut(writer);
            runnable.Invoke();
        }
        finally
        {
            // Recover the standard output stream so that a
            // completion message can be displayed.
            // var stdout = new StreamWriter(Console.OpenStandardOutput())
            // {
            //     AutoFlush = true
            // };
            Console.SetOut(stdout);
        }
    }

}

