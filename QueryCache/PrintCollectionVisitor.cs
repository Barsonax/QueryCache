using System.Collections;
using System.Linq.Expressions;

namespace QueryCache;

internal sealed class PrintCollectionVisitor : ExpressionVisitor
{
    protected override Expression VisitConstant(ConstantExpression node) =>
        node switch
        {
            { Value: string } => node,
            { Value: IQueryable } => node,
            { Value: IEnumerable enumerable } => CreateExpressionPrinter(enumerable),
            _ => node
        };

    private Expression CreateExpressionPrinter(IEnumerable enumerable)
    {
        var printer = CreatePrinter(enumerable);
        return Expression.Constant(printer, printer.GetType());
    }

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

        public override string ToString() => $"{{{string.Join(",", this)}}}";
    }
}