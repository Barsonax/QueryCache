using System.Linq.Expressions;
using FluentAssertions;

namespace QueryCache.Tests;

public class ExpressionEvaluatorTests
{
    [Fact]
    public void Variable()
    {
        //Arrange
        var variable = Guid.NewGuid().ToString();
        
        Expression<Func<string>> expression = () => variable;

        //Act
        var result = ExpressionEvaluator.Evaluate(expression.Body);

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
        var result = ExpressionEvaluator.Evaluate(expression.Body);

        //Assert
        result.Should().Be(variableContainer.nestedVariable);
    }
    
    [Fact]
    public void NestedVariableWithMethodCall()
    {
        //Arrange
        var variableContainer = new
        {
            nestedVariable = new [] { "foobar54" }
        };
        
        Expression<Func<bool>> expression = () => variableContainer.nestedVariable.Contains("foobar54");

        //Act
        var result = ExpressionEvaluator.Evaluate(expression.Body);

        //Assert
        result.Should().Be(true);
    }
}