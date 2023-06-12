using System.Linq.Expressions;

namespace QueryCache;

internal class EvaluateVisitor : ExpressionVisitor
{
    public override Expression? Visit(Expression? node) =>
        node switch
        {
            MemberExpression memberExpression => memberExpression switch
            {
                { Expression.NodeType: ExpressionType.Parameter } => memberExpression,
                _ => Expression.Constant(Expression.Lambda(memberExpression).Compile(true).DynamicInvoke())
            },
            _ => base.Visit(node)
        };
}