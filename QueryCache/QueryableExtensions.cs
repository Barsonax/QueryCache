using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.Caching;

namespace QueryCache;

public static class QueryableExtensions
{
    /// <summary>
    /// Materializes the query and caches the result in memory.
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="cache"></param>
    /// <param name="cacheItemPolicy"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ReadOnlyCollection<T> Cache<T>(this IQueryable<T> queryable, MemoryCache cache, CacheItemPolicy cacheItemPolicy)
    {
        var key = queryable.CalculateCacheKey();
        var cacheItem = cache.GetCacheItem(key);
        
        if (cacheItem != null)
        {
            return (ReadOnlyCollection<T>)cacheItem.Value;
        }

        var result = queryable.ToList().AsReadOnly();
        cache.Add(key, result, cacheItemPolicy);
        return result;
    }

    private static readonly ExpressionVisitor[] EvaluationVisitors = {
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