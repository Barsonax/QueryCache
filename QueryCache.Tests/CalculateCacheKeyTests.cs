using FluentAssertions;

namespace QueryCache.Tests;

public class CalculateCacheKeyTests
{
    [Fact]
    public void VariablesAreEvaluated()
    {
        //Arrange
        var variable = Guid.NewGuid().ToString();
        var query = new List<Account>().AsQueryable()
            .Where(x => x.Name == variable);
        
        //Act
        var key = query.CalculateCacheKey();
        
        //Assert
        key.Should().Contain(variable);
    }
    
    [Fact]
    public void VariablesAreEvaluated_NestedProperty()
    {
        //Arrange
        var variable = Guid.NewGuid().ToString();
        var query = new List<Account>().AsQueryable()
            .Where(x => x.NestedAccount.Name == variable);
        
        //Act
        var key = query.CalculateCacheKey();
        
        //Assert
        key.Should().Contain(variable);
    }
    
    [Fact]
    public void VariablesAreEvaluated_NestedVariable()
    {
        //Arrange
        var variableContainer = new
        {
            nestedVariable = Guid.NewGuid().ToString()
        };
        var query = new List<Account>().AsQueryable()
            .Where(x => x.NestedAccount.Name == variableContainer.nestedVariable);
        
        //Act
        var key = query.CalculateCacheKey();
        
        //Assert
        key.Should().Contain(variableContainer.nestedVariable);
    }
    
    [Fact]
    public void ListsArePrinted()
    {
        //Arrange
        var containsData = new List<string>()
        {
            "Operator1",
            "Operator2"
        };

        var query = new List<Account>().AsQueryable()
            .Where(x => containsData.Contains(x.Name));
        
        //Act
        var key = query.CalculateCacheKey();
        
        //Assert
        key.Should().Contain("{Operator1,Operator2}");
    }
    
    [Fact]
    public void ArraysArePrinted()
    {
        //Arrange
        var containsData = new string[]
        {
            "Operator1",
            "Operator2"
        };

        var query = new List<Account>().AsQueryable()
            .Where(x => containsData.Contains(x.Name));
        
        //Act
        var key = query.CalculateCacheKey();
        
        //Assert
        key.Should().Contain("{Operator1,Operator2}");
    }
    
    [Fact]
    public void Where_CollectionsArePrinted_And_VariablesAreEvaluated()
    {
        //Arrange
        var containsData = new List<string>()
        {
            "Operator1",
            "Operator2"
        };
        var variable = Guid.NewGuid().ToString();
        
        
        var query = new List<Account>().AsQueryable()
            .Where(x => containsData.Contains(x.Name))
            .Where(x => x.Name == variable);
        
        //Act
        var key = query.CalculateCacheKey();
        
        //Assert
        key.Should().Contain("Operator1");
        key.Should().Contain("Operator2");
        key.Should().Contain(variable);
    }
    
    [Fact]
    public void Where_And_Join_CollectionsArePrinted_And_VariablesAreEvaluated()
    {
        //Arrange
        var containsData = new List<string>()
        {
            "Operator1",
            "Operator2"
        };
        var variable = Guid.NewGuid().ToString();

        var outer = new List<Account>().AsQueryable().Where(x => containsData.Contains(x.Name));
        
        var query = new List<Account>().AsQueryable()
            .Where(x => containsData.Contains(x.Name))
            .Where(x => x.Name == variable)
            .Join(outer, x => x.Name, x => x.Name, (account, account1) => account);
        
        //Act
        var key = query.CalculateCacheKey();
        
        //Assert
        key.Should().Contain("Operator1");
        key.Should().Contain("Operator2");
        key.Should().Contain(variable);
    }
}

public class Account
{
    public string Name { get; set; }
    public Account NestedAccount { get; set; }
}