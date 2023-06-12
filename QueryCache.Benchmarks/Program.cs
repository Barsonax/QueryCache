// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using QueryCache;

BenchmarkRunner.Run<CalculateCacheKeyBenchmarks>();

[MemoryDiagnoser]
public class CalculateCacheKeyBenchmarks
{
    private IQueryable<Account> Query { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var containsData = new List<string>()
        {
            "Operator1",
            "Operator2"
        };

        Query = new List<Account>().AsQueryable().Where(x => containsData.Contains(x.Name));
    }
    
    [Benchmark]
    public string CalculateCacheKey()
    {
        return Query.CalculateCacheKey();
    }
}

public class Account
{
    public string Name { get; set; }
    public List<string> Operators { get; set; }
}