namespace Examples.Xunit;

/// <summary>
///　Helper to mock <c>Console.WriteLine()</c> and run tests.
/// </summary>
public class ConsoleHelper
{
    /// <summary>
    /// Sets the specified text writer to <c>Console.WriteLine()</c> and performs the specified action.
    /// It will return to normal immediately after execution.
    /// </summary>
    /// <param name="writer">The <see cref="TextWriter" /> instance or mock.</param>
    /// <param name="runnable">The delegate to execute.</param>
    public static void RunWith(TextWriter writer, Action runnable)
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

