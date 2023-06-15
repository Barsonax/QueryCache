using System.Linq.Expressions;

namespace QueryCache;

internal sealed class EvaluateVisitor : ExpressionVisitor
{
    protected override Expression VisitMember(MemberExpression node) =>
        CanEvaluate(node) ? Expression.Constant(MemberExpressionEvaluator.Evaluate(node)) : node;

    private bool CanEvaluate(MemberExpression node) =>
        node switch
        {
            { Expression.NodeType: ExpressionType.Parameter } => false,
            { Expression: MemberExpression nestedMemberExpression } => CanEvaluate(nestedMemberExpression),
            _ => true
        };
}