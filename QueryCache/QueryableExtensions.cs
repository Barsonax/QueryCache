using System.Linq.Expressions;

namespace QueryCache;

public static class QueryableExtensions
{
    private static ExpressionVisitor[] EvaluationVisitors = {
        new EvaluateVisitor(),
        new PrintCollectionVisitor()
    };
    
    public static string CalculateCacheKey<T>(this IQueryable<T> queryable)
    {
        var expression = queryable.Expression;
        foreach (var visitor in EvaluationVisitors)
        {
            expression = visitor.Visit(expression);
        }

        return expression.ToString();
    }
}