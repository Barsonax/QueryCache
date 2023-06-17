using System.Runtime.Caching;
using FluentAssertions;

namespace QueryCache.Tests;

public class CacheTests
{
    [Fact]
    public void CacheReturnsSameResult()
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
}