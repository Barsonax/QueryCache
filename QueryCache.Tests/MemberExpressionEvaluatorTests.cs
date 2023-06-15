using System.Linq.Expressions;
using FluentAssertions;

namespace QueryCache.Tests;

public class MemberExpressionEvaluatorTests
{
    [Fact]
    public void Variable()
    {
        //Arrange
        var variable = Guid.NewGuid().ToString();
        
        Expression<Func<string>> expression = () => variable;

        //Act
        var result = MemberExpressionEvaluator.Evaluate((MemberExpression)expression.Body);

        //Assert
        result.Should().Be(variable);
    }
    
    [Fact]
    public void NestedVariable()
    {
        //Arrange
        var variableContainer = new
        {
            nestedVariable = Guid.NewGuid().ToString()
        };
        
        Expression<Func<string>> expression = () => variableContainer.nestedVariable;

        //Act
        var result = MemberExpressionEvaluator.Evaluate((MemberExpression)expression.Body);

        //Assert
        result.Should().Be(variableContainer.nestedVariable);
    }
}