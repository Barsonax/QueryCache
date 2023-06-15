using System.Linq.Expressions;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net70)]
public class MemberExpressionInlineBenchmarks
{
    private MemberExpression MemberExpression { get; set; }
    
    [GlobalSetup]
    public void Setup()
    {
        var variable = Guid.NewGuid().ToString();
        var query = new List<Account>().AsQueryable()
            .Where(x => x.Name == variable);

        MemberExpression = (MemberExpression)((BinaryExpression)((LambdaExpression)((UnaryExpression)((MethodCallExpression)query.Expression).Arguments[1]).Operand).Body).Right;
        
        
    }
    
    //((FieldInfo)foo.Member).GetValue(((ConstantExpression)foo.Expression).Value)
    
    [Benchmark]
    public object Reflection()
    {
        return ((FieldInfo)MemberExpression.Member).GetValue(((ConstantExpression)MemberExpression.Expression).Value);
    }
    
    [Benchmark]
    public Expression LambdaCompileStaticTypedInterpolation()
    {
        return Expression.Constant(Expression.Lambda<Func<string>>(MemberExpression).Compile(true).DynamicInvoke());
    }
    
    [Benchmark]
    public Expression LambdaCompileStaticTyped()
    {
        return Expression.Constant(Expression.Lambda<Func<string>>(MemberExpression).Compile().DynamicInvoke());
    }
    /*
    [Benchmark]
    public Expression LambdaCompile()
    {
        return Expression.Constant(Expression.Lambda(MemberExpression).Compile().DynamicInvoke());
    }
    
    [Benchmark]
    public Expression LambdaCompileInterpolation()
    {
        return Expression.Constant(Expression.Lambda(MemberExpression).Compile(true).DynamicInvoke());
    }*/
}