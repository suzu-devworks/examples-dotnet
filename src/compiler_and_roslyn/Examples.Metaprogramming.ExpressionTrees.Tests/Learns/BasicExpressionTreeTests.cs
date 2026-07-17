using System.Linq.Expressions;

namespace Examples.Metaprogramming.ExpressionTrees.Tests.Learns;

/// <summary>
/// Tests to study the expression tree.
/// </summary>
/// <see href=""/>
public partial class BasicExpressionTreeTests
{
    [Fact]
    public void Step1_SimpleArithmetic()
    {
        // 5 + 7 * 3
        Expression body =
            Expression.Add(
                Expression.Constant(5),
                Expression.Multiply(
                    Expression.Constant(7),
                    Expression.Constant(3)
                )
            );

        Expression<Func<int>> lambda = Expression.Lambda<Func<int>>(body);
        Func<int> func = lambda.Compile();

        int result = func();

        // Assertion.
        Assert.Equal(26, result);
    }

    [Fact]
    public void Step2_WithParameter()
    {
        // Lambda expressions can be converted to Expression.
        Expression<Func<int, int>> exp1 = x => x + 5;
        var func1 = exp1.Compile();

        int result1 = func1(10);

        // y * (x + 5)
        var para1 = Expression.Parameter(typeof(int), "y");
        var para2 = Expression.Parameter(typeof(int), "x");
        var body = Expression.Multiply(para1,
                    Expression.Add(para2, Expression.Constant(5))
                    );
        var exp2 = Expression.Lambda<Func<int, int, int>>(body, new[] { para1, para2 });
        var func2 = exp2.Compile();

        int result2 = func2(10, 20);

        // x(10) + 5 = 15.
        Assert.Equal(15, result1);

        // y(10) * (x(20) + 5) = 250.
        Assert.Equal(250, result2);
    }

    [Fact]
    public void Step3_InstanceCreation()
    {
        // Default construction.
        var creator1 = Expression.Lambda<Func<Animal>>(
                        Expression.New(typeof(Animal))
                        ).Compile();

        var animal1 = creator1.Invoke();
        Assert.Equal("Unknown", animal1.Name);

        // Parametrized construction.
        var argsType = new[] { typeof(string) };
        var ctor = typeof(Animal).GetConstructor(argsType);
        var args = argsType.Select(t => Expression.Parameter(t)).ToList();
        var creator2 = Expression.Lambda<Func<string, Animal>>(
                        Expression.New(ctor!, args),
                        args).Compile();

        var animal = creator2.Invoke("John");
        Assert.Equal("John", animal.Name);
    }

}
