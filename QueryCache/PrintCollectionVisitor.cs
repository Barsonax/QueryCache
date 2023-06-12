using System.Collections;
using System.Linq.Expressions;

namespace QueryCache;

internal class PrintCollectionVisitor : ExpressionVisitor
{
    public override Expression? Visit(Expression? node) =>
        node switch
        {
            ConstantExpression constantExpression => constantExpression switch
            {
                { Value: string } => constantExpression,
                { Value: IQueryable } => constantExpression,
                { Value: IEnumerable enumerable } => Expression.Constant(CreatePrinter(enumerable), enumerable.GetType()),
                _ => constantExpression
            },
            _ => base.Visit(node)
        };

    private object CreatePrinter(IEnumerable enumerable)
    {
        var elementType = GetCollectionType(enumerable.GetType());
        
        var printerType = typeof(PrintedList<>).MakeGenericType(elementType);
        return Activator.CreateInstance(printerType, enumerable)!;
    }
    
    private Type GetCollectionType(Type type) =>
        type switch
        {
            { IsArray: true } => type.GetElementType()!,
            _ => type.GenericTypeArguments[0]
        };

    private class PrintedList<T> : List<T>
    {
        public PrintedList(IEnumerable enumerable)
        {
            this.AddRange(enumerable.Cast<T>());
        }

        public override string ToString() => $"{{{string.Join(",", this)}}}" ;
    }
}