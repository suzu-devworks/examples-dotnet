using Examples.Analyzer.Roslyn.Tests.Tutorials.Verifiers;
using Examples.Analyzer.Roslyn.Tutorials;

namespace Examples.Analyzer.Roslyn.Tests.Tutorials;

using VerifyCS = CSharpAnalyzerVerifier<MakeConstAnalyzer>;

public class MakeConstAnalyzerTests
{
    [Fact]
    public async Task VariableIsAssigned_NoDiagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync("""
            using System;

            class Program
            {
                static void Main()
                {
                    int i = 0;
                    Console.WriteLine(i++);
                }
            }
            """);
    }

    [Fact]
    public async Task VariableIsAlreadyConst_NoDiagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync("""
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
    public async Task NoInitializer_NoDiagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync("""
            using System;

            class Program
            {
                static void Main()
                {
                    int i;
                    i = 0;
                    Console.WriteLine(i);
                }
            }
            """);
    }

    [Fact]
    public async Task InitializerIsNotConstant_NoDiagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync("""
            using System;

            class Program
            {
                static void Main()
                {
                    int i = DateTime.Now.DayOfYear;
                    Console.WriteLine(i);
                }
            }
            """);
    }

    [Fact]
    public async Task MultipleInitializers_NoDiagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync("""
            using System;

            class Program
            {
                static void Main()
                {
                    int i = 0, j = DateTime.Now.DayOfYear;
                    Console.WriteLine(i + j);
                }
            }
            """);
    }

    // [Fact]
    // public async Task DeclarationIsInvalid_NoDiagnostic()
    // {
    //     await VerifyCS.VerifyAnalyzerAsync("""
    //         using System;

    //         class Program
    //         {
    //             static void Main()
    //             {
    //                int x = "abc"; // CS0029
    //             }
    //         }
    //         """);
    // }

    [Fact]
    public async Task DeclarationIsNotString_NoDiagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync("""
            using System;

            class Program
            {
                static void Main()
                {
                    object s = "abc";
                }
            }
            """);
    }

    [Fact]
    public async Task LocalIntCouldBeConstant_Diagnostic()
    {
        var expected = VerifyCS.Diagnostic("MakeConst").WithSpan(7, 9, 7, 19).WithArguments("i");
        await VerifyCS.VerifyAnalyzerAsync("""
            using System;

            class Program
            {
                static void Main()
                {
                    int i = 0;
                    Console.WriteLine(i);
                }
            }
            """, expected);
    }

    [Fact]
    public async Task StringCouldBeConstant_Diagnostic()
    {
        var expected = VerifyCS.Diagnostic("MakeConst").WithSpan(7, 9, 7, 26).WithArguments("s");
        await VerifyCS.VerifyAnalyzerAsync("""
            using System;

            class Program
            {
                static void Main()
                {
                    string s = "abc";
                    Console.WriteLine(s);
                }
            }
            """, expected);
    }

    [Fact]
    public async Task VarIntDeclarationCouldBeConstant_Diagnostic()
    {
        var expected = VerifyCS.Diagnostic("MakeConst").WithSpan(7, 9, 7, 19).WithArguments("i");
        await VerifyCS.VerifyAnalyzerAsync("""
            using System;

            class Program
            {
                static void Main()
                {
                    var i = 0;
                    Console.WriteLine(i);
                }
            }
            """, expected);
    }

    [Fact]
    public async Task VarStringDeclarationCouldBeConstant_Diagnostic()
    {
        var expected = VerifyCS.Diagnostic("MakeConst").WithSpan(7, 9, 7, 23).WithArguments("s");
        await VerifyCS.VerifyAnalyzerAsync("""
            using System;

            class Program
            {
                static void Main()
                {
                    var s = "abc";
                    Console.WriteLine(s);
                }
            }
            """, expected);
    }
}
