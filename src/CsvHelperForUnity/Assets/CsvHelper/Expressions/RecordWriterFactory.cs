﻿// Copyright 2009-2017 Josh Close and Contributors
// This file is a part of CsvHelper and is dual licensed under MS-PL and Apache 2.0.
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html for MS-PL and http://opensource.org/licenses/Apache-2.0 for Apache 2.0.
// https://github.com/JoshClose/CsvHelper
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CsvHelper.Expressions
{
	/// <summary>
	/// Factory to create record writers.
	/// </summary>
    public class RecordWriterFactory
    {
		private readonly CsvWriter writer;

		/// <summary>
		/// Initializes a new instance using the given writer.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public RecordWriterFactory( CsvWriter writer )
		{
			this.writer = writer;
		}

		/// <summary>
		/// Creates a new record writer for the given record.
		/// </summary>
		/// <typeparam name="T">The type of the record.</typeparam>
		/// <param name="record">The record.</param>
		public virtual RecordWriter MakeRecordWriter<T>( T record )
		{
			var type = writer.GetTypeForRecord( record );

			ExpandoObject expandoObject = record as ExpandoObject;
			//if( record is ExpandoObject expandoObject )
			if( record !=  null)
			{
				return new ExpandoObjectRecordWriter( writer );
			}

			/* 
			IDynamicMetaObjectProvider dynamicObject = record as IDynamicMetaObjectProvider;
			//if( record is IDynamicMetaObjectProvider dynamicObject )
			if( record !=  null )
			{
				return new DynamicRecordWriter( writer );
			}
			*/

			if( type.GetTypeInfo().IsPrimitive )
			{
				return new PrimitiveRecordWriter( writer );
			}

			return new ObjectRecordWriter( writer );
		}
    }
}
