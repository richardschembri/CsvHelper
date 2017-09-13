# Reading

This library was created to work as easy as possible without any configuration by default. If your class property names match your CSV file header names, it's as simple as this.

```cs
var csv = new CsvReader( textReader );
var records = csv.GetRecords<MyClass>();
```

`GetRecords<T>()` returns an `IEnumerable<T>` that will `yield` results. This means when iterating  the results, only a single records will be in memory at a time, instead of the entire file.

