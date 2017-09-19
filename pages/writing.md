# Writing

This library was created to work as easy as possible without any configuration by default. If your class property names match your CSV file header names, it's as simple as this.

```cs
var records = new List<MyClass> { ... };
var csv = new CsvWriter( textWriter );
csv.WriteRecords( records );
```

### Writing All Records

<hr/>

The most common scenario is using the `WriteRecords` method. You can pass it an `IEnumerable` of records and it will write those objects.

#### WriteRecords

Writes all records.

```cs
var records = new List<MyClass>
{
	new MyClass { Id = 1, Name = "one" },
	new MyClass { Id = 2, Name = "two" },
};
csv.WriteRecords( records );
```

### Writing a Single Record

<hr/>

Sometimes you want to write individual records by themselves.

#### WriteHeader

Writes the header record. You can call this method on any row if you want to write multiple headers.

```cs
csv.WriteHeader<MyClass>();
csv.WriteHeader( Type type );
```

#### WriteRecord

Writes a record.

```cs
csv.WriteRecord<MyClass>( record );
```

### Writing Fields

<hr/>

You can even write a single field.

#### WriteField

Write any type of object to a field. You can specify your own `ITypeConverter` to handle converting the type to a string if none of the built in converters work for you.

```cs
csv.WriteField( "field" );
csv.WriteField( 1 );
csv.WriteField( value, myTypeConverter );
```

#### WriteComment

This will write text to the field using the comment character supplied in `Configuration.Comment`.

```cs
csv.WriteComment( "This is a comment. ");
```

### Ending the Row

<hr/>

When you are done writing the row, you need to flush the fields and start a new row. Flushing and starting a new row are separated so that you can flush without creating a new row.

#### Flush

Serialize the fields to the `TextReader`.

```cs
csv.Flush();
```

#### FlushAsync

Serialize the fields to the `TextReader` asynchronously. If the `TextReader` supplied is tied to a network or some other slow to write functionality, flushing asynchronously is probably a good idea.

```cs
csv.FlushAsync();
```

#### NextRecord

Ends the current record and starts a new record. This will call `Flush` then write a newline.

```cs
csv.NextRecord();
```

#### NextRecordAsync

Ends the current record and start a new record asynchronously. This will call `FlushAsync` then asynchronously write a newline.

```cs
csv.NextRecordAsync();
```

### Writing Context

<hr/>

When writing, all the information in the system is held in a context object. If you need to get raw system information for some reason, it's available here. When an exception is throw, the context is included so you can inspect the current state of the writer.

### Configuration

<hr/>

See [configuration](/configuration)

<br/>
