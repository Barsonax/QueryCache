using System.Reflection;

namespace QueryableCacheDotnet;

public static class ExpressionEvaluator
{
    public static object Evaluate(Expression expression) => expression switch
    {
        MemberExpression { Expression: ConstantExpression constantExpression } memberExpression => GetMemberValue(memberExpression.Member, constantExpression.Value),
        MemberExpression memberExpression => GetMemberValue(memberExpression.Member, Evaluate(memberExpression.Expression)),
        _ => Expression.Lambda(expression).Compile(false).DynamicInvoke() // Compiling the expression is expensive, especially on netframework so we only do it if there is not a easy shortcut.
    };

#pragma warning disable CS8509
    private static object GetMemberValue(MemberInfo memberInfo, object instance) => memberInfo switch
#pragma warning restore CS8509
    {
        FieldInfo fieldInfo => fieldInfo.GetValue(instance),
        PropertyInfo propertyInfo => propertyInfo.GetValue(instance),
    };
}