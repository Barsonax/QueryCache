# QueryCache

I had a idea to create a extension method on a IQueryable<T> that caches the query output. I believed this should be possible as a IQueryable<T> is really just a expression tree that gets translated to SQL. By using the expression tree to calculate a cache key it does not matter what ORM is used as long as it uses IQueryable<T>. Furthermore a extension method on IQueryable<T> gives you alot of flexibility on when to apply caching and also works very well with method chaining.

## Benchmarks
As calculating the cache keys should be as fast as possible benchmarks have been added in order to measure the time this takes:

```
|            Method |                Job |            Runtime | WhereClauses |       Mean |     Error |    StdDev |    Gen0 |   Gen1 | Allocated |
|------------------ |------------------- |------------------- |------------- |-----------:|----------:|----------:|--------:|-------:|----------:|
| CalculateCacheKey |           .NET 7.0 |           .NET 7.0 |            1 |   7.025 us | 0.0403 us | 0.0377 us |  0.4807 |      - |   3.97 KB |
| CalculateCacheKey | .NET Framework 4.8 | .NET Framework 4.8 |            1 | 186.305 us | 1.0505 us | 0.9826 us |  2.9297 | 1.4648 |  19.46 KB |
| CalculateCacheKey |           .NET 7.0 |           .NET 7.0 |            2 |  13.702 us | 0.0745 us | 0.0697 us |  0.8698 |      - |   7.19 KB |
| CalculateCacheKey | .NET Framework 4.8 | .NET Framework 4.8 |            2 | 358.398 us | 1.0477 us | 0.8749 us |  5.8594 | 2.9297 |  38.16 KB |
| CalculateCacheKey |           .NET 7.0 |           .NET 7.0 |            3 |  20.006 us | 0.1352 us | 0.1265 us |  1.3123 |      - |  10.81 KB |
| CalculateCacheKey | .NET Framework 4.8 | .NET Framework 4.8 |            3 | 534.311 us | 6.7726 us | 6.3351 us |  8.7891 | 3.9063 |  57.26 KB |
| CalculateCacheKey |           .NET 7.0 |           .NET 7.0 |            4 |  26.305 us | 0.1354 us | 0.1266 us |  1.7090 |      - |  14.03 KB |
| CalculateCacheKey | .NET Framework 4.8 | .NET Framework 4.8 |            4 | 698.614 us | 4.4373 us | 4.1506 us | 11.7188 | 5.8594 |  75.95 KB |
| CalculateCacheKey |           .NET 7.0 |           .NET 7.0 |            5 |  33.525 us | 0.3141 us | 0.2623 us |  2.0752 |      - |  17.24 KB |
| CalculateCacheKey | .NET Framework 4.8 | .NET Framework 4.8 |            5 | 868.777 us | 4.2640 us | 3.9886 us | 14.6484 | 6.8359 |  94.64 KB |

```
