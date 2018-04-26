using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;

namespace CsvHelper.Tests.AutoMapping
{
	[TestClass]
    public class FSharpTests
    {
		[TestMethod]
        public void NestedTest()
		{
			var config = new CsvHelper.Configuration.Configuration();
			var map = config.AutoMap( FSharpRecords.nestedRecords[0].GetType() );
		}

		[TestMethod]
		public void WriteNestedTest()
		{
			using( var stream = new MemoryStream() )
			using( var reader = new StreamReader( stream ) )
			using( var writer = new StreamWriter( stream ) )
			using( var csv = new CsvWriter( writer ) )
			{
				csv.WriteRecords( FSharpRecords.nestedRecords );
				writer.Flush();
				stream.Position = 0;

				var expected = new StringBuilder();
				expected.AppendLine( "key,a,b" );
				expected.AppendLine( "one,a1,b1" );
				expected.AppendLine( "two,a2,b2" );

				var result = reader.ReadToEnd();

				Assert.AreEqual( expected.ToString(), result );
			}
		}

		[TestMethod]
		public void ReadNestedTest()
		{
			using( var stream = new MemoryStream() )
			using( var reader = new StreamReader( stream ) )
			using( var writer = new StreamWriter( stream ) )
			using( var csv = new CsvReader( reader ) )
			{
				writer.WriteLine( "key,a,b" );
				writer.WriteLine( "one,a1,b1" );
				writer.WriteLine( "two,a2,b2" );
				writer.Flush();
				stream.Position = 0;

				var records = csv.GetRecords<FSharpRecords.Container>().ToList();
			}
		}
    }
}
