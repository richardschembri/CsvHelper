// Copyright 2009-2017 Josh Close and Contributors
// This file is a part of CsvHelper and is dual licensed under MS-PL and Apache 2.0.
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html for MS-PL and http://opensource.org/licenses/Apache-2.0 for Apache 2.0.
// https://github.com/JoshClose/CsvHelper
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvHelper.Configuration
{
	/// <summary>
	/// A collection that holds <see cref="ParameterMap"/>s.
	/// </summary>
	public class ParameterMapCollection : IList<ParameterMap>
	{
		private readonly List<ParameterMap> list = new List<ParameterMap>();
		private readonly IComparer<ParameterMap> comparer;

		/// <summary>
		/// Gets the <see cref="ParameterMap"/> at the specified index.
		/// </summary>
		/// <value>
		/// The <see cref="ParameterMap"/>.
		/// </value>
		/// <param name="index">The index.</param>
		public ParameterMap this[int index]
		{
			get { return list[index]; }
			set { list[index] = value;  }
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		public int Count => list.Count;

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
		/// </summary>
		public bool IsReadOnly => false;

		/// <summary>
		/// Initializes a new instance of the <see cref="ParameterMapCollection"/> class.
		/// </summary>
		public ParameterMapCollection() : this( new ParameterMapComparer() ) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ParameterMapCollection"/> class.
		/// </summary>
		/// <param name="comparer">The comparer.</param>
		public ParameterMapCollection( IComparer<ParameterMap> comparer )
		{
			this.comparer = comparer;
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		public void Add( ParameterMap item )
		{
			list.Add( item );
			list.Sort( comparer );
		}

		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="collection">The collection.</param>
		public void AddRange( ICollection<ParameterMap> collection )
		{
			list.AddRange( collection );
			list.Sort( comparer );
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		public void Clear()
		{
			list.Clear();
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>
		/// true if <paramref name="item">item</paramref> is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
		/// </returns>
		public bool Contains( ParameterMap item )
		{
			return list.Contains( item );
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		public void CopyTo( ParameterMap[] array, int arrayIndex )
		{
			list.CopyTo( array, arrayIndex );
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// An enumerator that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<ParameterMap> GetEnumerator()
		{
			return list.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"></see>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
		/// <returns>
		/// The index of <paramref name="item">item</paramref> if found in the list; otherwise, -1.
		/// </returns>
		public int IndexOf( ParameterMap item )
		{
			return list.IndexOf( item );
		}

		/// <summary>
		/// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"></see> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
		public void Insert( int index, ParameterMap item )
		{
			list.Insert( index, item );
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>
		/// true if <paramref name="item">item</paramref> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if <paramref name="item">item</paramref> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </returns>
		public bool Remove( ParameterMap item )
		{
			return list.Remove( item );
		}

		/// <summary>
		/// Removes the <see cref="T:System.Collections.Generic.IList`1"></see> item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		public void RemoveAt( int index )
		{
			list.RemoveAt( index );
		}

		/// <summary>
		/// Adds the parameters from the mapping. This will recursively
		/// traverse the mapping tree and add all parameters for
		/// reference maps.
		/// </summary>
		/// <param name="map">The mapping where the members are added from.</param>
		public void AddParameters( ClassMap map )
		{
			AddRange( map.ParameterMaps );
			foreach( var refmap in map.ReferenceMaps )
			{
				AddParameters( refmap.Data.Mapping );
			}
		}
	}
}
