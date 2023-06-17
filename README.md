# QueryableCacheDotnet

I had a idea to create a extension method on a IQueryable<T> that caches the query output. I believed this should be possible as a IQueryable<T> is really just a expression tree that gets translated to SQL. By using the expression tree to calculate a cache key it does not matter what ORM is used as long as it uses IQueryable<T>. Furthermore a extension method on IQueryable<T> gives you alot of flexibility on when to apply caching and also works very well with method chaining.

## Benchmarks
As calculating the cache keys should be as fast as possible benchmarks have been added in order to measure the time this takes:

```
BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.3086/22H2/2022Update)
AMD Ryzen 9 3900X, 1 CPU, 24 logical and 12 physical cores
  [Host]             : .NET Framework 4.8 (4.8.4644.0), X64 RyuJIT VectorSize=256
  .NET 7.0           : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  .NET Framework 4.8 : .NET Framework 4.8 (4.8.4644.0), X64 RyuJIT VectorSize=256


|            Method |                Job |            Runtime | WhereClauses |      Mean |     Error |    StdDev |   Gen0 | Allocated |
|------------------ |------------------- |------------------- |------------- |----------:|----------:|----------:|-------:|----------:|
| CalculateCacheKey |           .NET 7.0 |           .NET 7.0 |            1 |  4.737 us | 0.0177 us | 0.0165 us | 0.3433 |   2.81 KB |
| CalculateCacheKey | .NET Framework 4.8 | .NET Framework 4.8 |            1 | 17.715 us | 0.0377 us | 0.0315 us | 0.7629 |   4.88 KB |
| CalculateCacheKey |           .NET 7.0 |           .NET 7.0 |            2 |  8.923 us | 0.0322 us | 0.0301 us | 0.5951 |   4.88 KB |
| CalculateCacheKey | .NET Framework 4.8 | .NET Framework 4.8 |            2 | 34.120 us | 0.0942 us | 0.0881 us | 1.4038 |   8.99 KB |
| CalculateCacheKey |           .NET 7.0 |           .NET 7.0 |            3 | 13.042 us | 0.0374 us | 0.0331 us | 0.8850 |   7.34 KB |
| CalculateCacheKey | .NET Framework 4.8 | .NET Framework 4.8 |            3 | 50.303 us | 0.1524 us | 0.1425 us | 2.1362 |  13.49 KB |
| CalculateCacheKey |           .NET 7.0 |           .NET 7.0 |            4 | 17.254 us | 0.0652 us | 0.0610 us | 1.1292 |    9.4 KB |
| CalculateCacheKey | .NET Framework 4.8 | .NET Framework 4.8 |            4 | 65.648 us | 0.2358 us | 0.2206 us | 2.8076 |  17.61 KB |
| CalculateCacheKey |           .NET 7.0 |           .NET 7.0 |            5 | 21.377 us | 0.0819 us | 0.0766 us | 1.3733 |  11.45 KB |
| CalculateCacheKey | .NET Framework 4.8 | .NET Framework 4.8 |            5 | 81.581 us | 0.1786 us | 0.1583 us | 3.4180 |  21.72 KB |

```
