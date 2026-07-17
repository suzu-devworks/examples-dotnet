using Examples.Analyzer.Roslyn.CodeFixes.Tutorials;
using Examples.Analyzer.Roslyn.Tests.Tutorials.Verifiers;
using Examples.Analyzer.Roslyn.Tutorials;

namespace Examples.Analyzer.Roslyn.Tests.Tutorials;

using VerifyCS = CSharpCodeFixVerifier<MakeConstAnalyzer, MakeConstCodeFixProvider>;

public class MakeConstCodeFixProviderTests
{
    [Fact]
    public async Task LocalIntCouldBeConstant_Diagnostic()
    {
        var expected = VerifyCS.Diagnostic("MakeConst").WithSpan(7, 9, 7, 19).WithArguments("i");
        await VerifyCS.VerifyCodeFixAsync("""
            using System;

            class Program
            {
                static void Main()
                {
                    int i = 0;
                    Console.WriteLine(i);
                }
            }
            """, expected, """
            using System;

            class Program
            {
                static void Main()
                {
                    const int i = 0;
                    Console.WriteLine(i);
                }
            }
            """);
    }

    [Fact]
    public async Task StringCouldBeConstant_Diagnostic()
    {
        var expected = VerifyCS.Diagnostic("MakeConst").WithSpan(7, 9, 7, 26).WithArguments("s");
        await VerifyCS.VerifyCodeFixAsync("""
            using System;

            class Program
            {
                static void Main()
                {
                    string s = "abc";
                    Console.WriteLine(s);
                }
            }
            """, expected, """
            using System;

            class Program
            {
                static void Main()
                {
                    const string s = "abc";
                    Console.WriteLine(s);
                }
            }
            """);
    }

    [Fact]
    public async Task VarIntDeclarationCouldBeConstant_Diagnostic()
    {
        var expected = VerifyCS.Diagnostic("MakeConst").WithSpan(7, 9, 7, 19).WithArguments("i");
        await VerifyCS.VerifyCodeFixAsync("""
            using System;

            class Program
            {
                static void Main()
                {
                    var i = 0;
                    Console.WriteLine(i);
                }
            }
            """, expected, """
            using System;

            class Program
            {
                static void Main()
                {
                    const int i = 0;
                    Console.WriteLine(i);
                }
            }
            """);
    }

    [Fact]
    public async Task VarStringDeclarationCouldBeConstant_Diagnostic()
    {
        var expected = VerifyCS.Diagnostic("MakeConst").WithSpan(7, 9, 7, 23).WithArguments("s");
        await VerifyCS.VerifyCodeFixAsync("""
            using System;

            class Program
            {
                static void Main()
                {
                    var s = "abc";
                    Console.WriteLine(s);
                }
            }
            """, expected, """
            using System;

            class Program
            {
                static void Main()
                {
                    const string s = "abc";
                    Console.WriteLine(s);
                }
            }
            """);
    }
}
