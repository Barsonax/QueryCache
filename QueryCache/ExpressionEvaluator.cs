using System.Linq.Expressions;
using System.Reflection;
using CommunityToolkit.Diagnostics;

namespace QueryCache;

public static class ExpressionEvaluator
{
    public static object Evaluate(Expression expression) => expression switch
    {
        MemberExpression { Expression: ConstantExpression constantExpression } memberExpression => GetMemberValue(memberExpression.Member, constantExpression.Value),
        MemberExpression memberExpression => GetMemberValue(memberExpression.Member, Evaluate(memberExpression.Expression)),
        _ => Expression.Lambda(expression).Compile(false).DynamicInvoke() // Compiling the expression is expensive, especially on netframework so we only do it if there is not a easy shortcut.
    };

    private static object GetMemberValue(MemberInfo memberInfo, object instance) => memberInfo switch
    {
        FieldInfo fieldInfo => fieldInfo.GetValue(instance),
        PropertyInfo propertyInfo => propertyInfo.GetValue(instance),
        _ => ThrowArgumentOutOfRangeException(nameof(memberInfo), memberInfo, memberInfo.GetType().Name)
    };
    
    private static object ThrowArgumentOutOfRangeException(string? name, object? value, string? message)
    {
        ThrowHelper.ThrowArgumentOutOfRangeException(name, value, message);
        return null;
    }
}