using System.Runtime.Caching;

namespace QueryableCacheDotnet.Tests;

public class CacheTests
{
    [Fact]
    public void Cache_SameQuery_ReturnsSameResult()
    {
        //Arrange
        var cache = new MemoryCache("querycache");

        var data = new List<Account>()
        {
            new Account()
        };
        
        var query = data.AsQueryable()
            .Where(x => x.Name == null);
        
        //Act
        var cachedResult1 = query.Cache(cache, new CacheItemPolicy());
        var cachedResult2 = query.Cache(cache, new CacheItemPolicy());
        
        //Assert
        cachedResult1.Should().BeSameAs(cachedResult2);
    }
    
    [Fact]
    public void Cache_DifferentQuery_ReturnsDifferentResult()
    {
        //Arrange
        var cache = new MemoryCache("querycache");

        var data = new List<Account>()
        {
            new Account()
        };
        
        var query1 = data.AsQueryable()
            .Where(x => x.Name == null);
        
        var query2 = data.AsQueryable()
            .Where(x => x.Name == null)
            .Where(x => x.Name == null);
        
        //Act
        var cachedResult1 = query1.Cache(cache, new CacheItemPolicy());
        var cachedResult2 = query2.Cache(cache, new CacheItemPolicy());
        
        //Assert
        cachedResult1.Should().NotBeSameAs(cachedResult2);
    }
}