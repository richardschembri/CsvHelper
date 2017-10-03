# Class Mapping

Sometimes your class members and CSV headers don't match up. Sometimes your CSV files don't have a header row and you need to specify an index for a member [because you can't rely on the ordering of class members in .NET](https://blogs.msdn.microsoft.com/haibo_luo/2006/07/10/member-order-returned-by-getfields-getmethods/). For these situations you can create a class map that maps a class member to a CSV field using the configuration you specify.

To create a mapping from a class to a CSV file, you use a `ClassMap`. You can map any public members (properties or fields).

```cs
public class MyClass
{
	public int Id { get; set; }
	public string Name { get; set; }
}

public sealed class MyClassMap : ClassMap<MyClass>
{
	public MyClassMap()
	{
		Map( m => m.Id );
		Map( m => m.Name );
	}
}
```

To use this mapping, you need to register the mapping in the configuration.

```cs
var csv = new CsvReader( textReader );
csv.Configuration.RegisterClassMap<MyClassMap>();
```

## Reference Mapping

<hr/>

To map a reference member, just reference the member down the tree. You can reference as far down the tree as you need.

```cs
public class A
{
	public int Id { get; set; }
	public B B { get; set; }
}

public class B
{
	public int Id { get; set; }
	public C C { get; set; }
}

public class C
{
	public int Id { get; set; }
}

public sealed class AMap : ClassMap<A>
{
	public AMap()
	{
		Map( m => m.Id ).Name( "A" );
		Map( m => m.B.Id ).Name( "B" );
		Map( m => m.B.C.Id).Name( "C" );
	}
}
```

What reference member mapping does is creates a reference map. You can manually create a reference map like this.

```cs
public class A
{
	public int Id { get; set; }
	public B B { get; set; }
}

public class B
{
	public int Id { get; set; }
	public C C { get; set; }
}

public class C
{
	public int Id { get; set; }
}

public sealed class AMap : ClassMap<A>
{
	public AMap()
	{
		Map( m => m.Id ).Name( "A" );
		References<BMap>( m => m.B );
	}
}

public sealed class BMap : ClassMap<B>
{
	public BMap()
	{
		Map( m => m.Id ).Name( "B" );
		References<CMap>( m => m.C );
	}
}

public sealed class CMap : ClassMap<C>
{
	public CMap()
	{
		Map( m => m.Id ).Name( "C" );
	}
}
```

## Auto Mapping

<hr/>

If you don't supply a mapping file and try to read or write, a mapping file is automatically created for you through auto mapping. Auto mapping will traverse the object graph and create member mappings for you using defaults. You can change some of these defaults through configuration. If a circular reference is detected, the auto mapper will stop traversing that tree node and continue with the next.

You can also call `AutoMap` in your `ClassMap`. If you only have a few changes you want to make, you can use `AutoMap` to create the initial map, then make only the changes you want.

```cs
public class MyClass
{
	public int Id { get; set; }

	public string Name { get; set; }

	public DateTime CreatedDate { get; set; }
}

public sealed class MyClassMap : ClassMap<MyClass>
{
	public MyClassMap()
	{
		AutoMap();
		Map( m => m.CreatedDate ).Ignore();
	}
}
```

## Options

<hr/>

You are able change the behavior of the member mapping through options.

### Name

Specifies the name of the field header. You can pass in multiple names if the field might have more than one name that is used for it when reading. All the names will be checked when looking for the field. When writing, only the first name is used.

```cs
// Single name
Map( m => m.Id ).Name( "id" );

// Multiple possible names
Map( m => m.Id ).Name( "id", "the_id", "Id" );
```

### NameIndex

Specifies the zero-based index of the header name if the header name appears in more than one column.

```cs
// Example header
id,name,id

// Mapping
Map( m => m.Id ).Name( "id" ).Index( 1 );
```

### Index

Specifies the zero-based index of the field. When reading this is used if there is no header. A name will override an index. When writing you can specify both so that the position of the column is guaranteed.

```cs
Map( m => m.Id ).Index( 0 );
```

### Default

### Constant

### Ignore

### TypeConverter

### ConvertUsing

### Validate

## FAQ

<hr/>

### How Do I Map Private Fields?

<br/>
