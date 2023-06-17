using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace QueryableCacheDotnet.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net70)]
public class CalculateCacheKeyBenchmarks
{
    private IQueryable<Account> Query { get; set; }

    [Params(1, 2, 3, 4, 5)]
    public int WhereClauses;

    [GlobalSetup]
    public void Setup()
    {
        var containsData = new List<string>()
        {
            "Operator1",
            "Operator2"
        };

        Query = new List<Account>().AsQueryable();

        for (int i = 0; i < WhereClauses; i++)
        {
            Query = Query.Where(x => containsData.Contains(x.Name));
        }
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