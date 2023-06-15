using System.Linq.Expressions;
using System.Reflection;

namespace QueryCache;

public static class MemberExpressionEvaluator
{
    public static object Evaluate(MemberExpression memberExpression) => EvaluateRecursive(memberExpression);
    
    private static object EvaluateRecursive(Expression expression) => expression switch
        {
            MemberExpression {Expression: ConstantExpression constantExpression} memberExpression => GetMemberValue(memberExpression.Member, constantExpression.Value),
            MemberExpression memberExpression => GetMemberValue(memberExpression.Member, EvaluateRecursive(memberExpression.Expression)),
            _ => throw new ArgumentOutOfRangeException()
        };
    
    private static object GetMemberValue(MemberInfo memberInfo, object instance) => memberInfo switch
        {
            FieldInfo fieldInfo => fieldInfo.GetValue(instance),
            PropertyInfo propertyInfo => propertyInfo.GetValue(instance),
            _ => throw new ArgumentOutOfRangeException()
        };
}