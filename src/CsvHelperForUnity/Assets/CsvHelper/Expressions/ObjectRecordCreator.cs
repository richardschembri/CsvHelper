﻿// Copyright 2009-2017 Josh Close and Contributors
// This file is a part of CsvHelper and is dual licensed under MS-PL and Apache 2.0.
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html for MS-PL and http://opensource.org/licenses/Apache-2.0 for Apache 2.0.
// https://github.com/JoshClose/CsvHelper
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CsvHelper.Expressions
{
	/// <summary>
	/// Creates objects.
	/// </summary>
	public class ObjectRecordCreator : RecordCreator
	{
		/// <summary>
		/// Initializes a new instnace using the given reader.
		/// </summary>
		/// <param name="reader"></param>
		public ObjectRecordCreator( CsvReader reader ) : base( reader ) { }

		/// <summary>
		/// Creates a <see cref="Delegate"/> of type <see cref="Func{T}"/>
		/// that will create a record of the given type using the current
		/// reader row.
		/// </summary>
		/// <param name="recordType">The record type.</param>
		protected override Delegate CreateCreateRecordDelegate( Type recordType )
		{
			if( Reader.Context.ReaderConfiguration.Maps[recordType] == null )
			{
				Reader.Context.ReaderConfiguration.Maps.Add( Reader.Context.ReaderConfiguration.AutoMap( recordType ) );
			}

			var map = Reader.Context.ReaderConfiguration.Maps[recordType];

			Expression body;

			if( map.ParameterMaps.Count > 0 )
			{
				// This is a constructor paramter type.
				var arguments = new List<Expression>();
				CreateConstructorArgumentExpressionsForMapping( map, arguments );

				body = Expression.New( Reader.Configuration.GetConstructor( map.ClassType ), arguments );
			}
			else
			{
				var assignments = new List<MemberAssignment>();
				ExpressionManager.CreateMemberAssignmentsForMapping( map, assignments );

				if( assignments.Count == 0 )
				{
					throw new ReaderException( Reader.Context, $"No members are mapped for type '{recordType.FullName}'." );
				}

				body = ExpressionManager.CreateInstanceAndAssignMembers( recordType, assignments );
			}

			var funcType = typeof( Func<> ).MakeGenericType( recordType );

			return Expression.Lambda( funcType, body ).Compile();
		}

		/// <summary>
		/// Creates the constructor arguments used to create a type.
		/// </summary>
		/// <param name="map">The mapping to create the arguments for.</param>
		/// <param name="argumentExpressions">The arguments that will be added to the mapping.</param>
		protected virtual void CreateConstructorArgumentExpressionsForMapping( ClassMap map, List<Expression> argumentExpressions )
		{
			foreach( var parameterMap in map.ParameterMaps )
			{
				if( parameterMap.ConstructorTypeMap != null )
				{
					// Constructor paramter type.
					var arguments = new List<Expression>();
					CreateConstructorArgumentExpressionsForMapping( parameterMap.ConstructorTypeMap, arguments );
					var constructorExpression = Expression.New( Reader.Configuration.GetConstructor( parameterMap.ConstructorTypeMap.ClassType ), arguments );

					argumentExpressions.Add( constructorExpression );
				}
				else if( parameterMap.ReferenceMap != null )
				{
					// Reference type.

					var referenceAssignments = new List<MemberAssignment>();
					ExpressionManager.CreateMemberAssignmentsForMapping( parameterMap.ReferenceMap.Data.Mapping, referenceAssignments );

					var referenceBody = ExpressionManager.CreateInstanceAndAssignMembers( parameterMap.ReferenceMap.Data.Parameter.ParameterType, referenceAssignments );
					argumentExpressions.Add( referenceBody );
				}
				else
				{
					// Value type.

					var index = Reader.Configuration.HasHeaderRecord
						? Reader.GetFieldIndex( parameterMap.Data.Name, 0 )
						: parameterMap.Data.Index;

					// Get the field using the field index.
					var method = typeof( IReaderRow ).GetProperty( "Item", typeof( string ), new[] { typeof( int ) } ).GetGetMethod();
					Expression fieldExpression = Expression.Call( Expression.Constant( Reader ), method, Expression.Constant( index, typeof( int ) ) );

					// Convert the field.
					var typeConverterExpression = Expression.Constant( parameterMap.Data.TypeConverter );
					parameterMap.Data.TypeConverterOptions = TypeConverterOptions.Merge( new TypeConverterOptions { CultureInfo = Reader.Context.ReaderConfiguration.CultureInfo }, Reader.Context.ReaderConfiguration.TypeConverterOptionsCache.GetOptions( parameterMap.Data.Parameter.ParameterType ), parameterMap.Data.TypeConverterOptions );

					// Create type converter expression.
					var memberMapData = new MemberMapData( null )
					{
						Index = parameterMap.Data.Index,
						TypeConverter = parameterMap.Data.TypeConverter,
						TypeConverterOptions = parameterMap.Data.TypeConverterOptions
					};
					memberMapData.Names.Add( parameterMap.Data.Name );
					Expression typeConverterFieldExpression = Expression.Call( typeConverterExpression, nameof( ITypeConverter.ConvertFromString ), null, fieldExpression, Expression.Constant( Reader ), Expression.Constant( memberMapData ) );
					typeConverterFieldExpression = Expression.Convert( typeConverterFieldExpression, parameterMap.Data.Parameter.ParameterType );

					fieldExpression = typeConverterFieldExpression;

					argumentExpressions.Add( fieldExpression );
				}
			}
		}
	}
}
