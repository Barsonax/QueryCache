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
    public void CollectionsArePrinted()
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
    public void Where_CollectionsArePrinted_And_VariablesAreEvaluated_2()
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
    public List<string> Operators { get; set; }
}