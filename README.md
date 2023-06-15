# QueryCache

I had a idea to create a extension method on a IQueryable<T> that caches the query output. I believed this should be possible as a IQueryable<T> is really just a expression tree that gets translated to SQL. By using the expression tree to calculate a cache key it does not matter what ORM is used as long as it uses IQueryable<T>. Furthermore a extension method on IQueryable<T> gives you alot of flexibility on when to apply caching and also works very well with method chaining.
