﻿using System;
using System.Collections.Generic;
using Whathecode.System.Arithmetic.Range;
using Whathecode.System.Collections.Algorithm;
using Whathecode.System.Collections.Delegates;


namespace Whathecode.System.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		/// Does a binary search on the keys of a generic SortedList.
		/// </summary>
		/// <typeparam name = "TKey">The key type of the SortedList.</typeparam>
		/// <typeparam name = "TValue">The value type of the SortedList.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "key">The key to look for.</param>
		/// <returns>The search result, containing the found value, and/or the nearest data.</returns>
		public static BinarySearchResult<TKey> BinarySearchKeys<TKey, TValue>( this SortedList<TKey, TValue> source, TKey key )
			where TKey : IComparable<TKey>
		{
			if ( source.Count <= 0 )
			{
				throw new InvalidOperationException( "In order to do a binary search at least one item most be inside the list." );
			}

			return Collections.Algorithm.BinarySearch<TKey, int>.Search(
				key,
				source.Keys.GetIndexInterval(), // The interval in which to search.
				new IndexerDelegates<TKey, int>(
					index => source.Keys[ index ], // Delegate to get keys by index.
					index => index // Delegate to get the nearest correct index.
					) );
		}

		/// <summary>
		/// Does a binary search in a generic SortedList by looking up a key.
		/// </summary>
		/// <typeparam name = "TKey">The key type of the SortedList.</typeparam>
		/// <typeparam name = "TValue">The value type of the SortedList.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "key">The key to look for.</param>
		/// <returns>The search result, containing the found value, and/or the nearest data.</returns>
		public static BinarySearchResult<TValue> BinarySearch<TKey, TValue>( this SortedList<TKey, TValue> source, TKey key )
			where TKey : IComparable<TKey>
		{
			if ( source.Count <= 0 )
			{
				throw new InvalidOperationException( "In order to do a binary search at least one item most be inside the list." );
			}

			// Do a binary search on the keys, and retrieve the values associated with the keys.
			BinarySearchResult<TKey> searchResult = BinarySearchKeys( source, key );
			var valueResult = new BinarySearchResult<TValue>
			{
				IsObjectInRange = searchResult.IsObjectInRange,
				IsObjectFound = searchResult.IsObjectFound,
				Found = searchResult.IsObjectFound
					? new BinarySearchResult<TValue>.FoundResult( source[ searchResult.Found.Object ] )
					: null,
				NotFound = searchResult.IsObjectFound
					? null
					: new BinarySearchResult<TValue>.NotFoundResult( source[ searchResult.NotFound.Smaller ], source[ searchResult.NotFound.Bigger ] )
			};

			return valueResult;
		}

		/// <summary>
		/// Get the interval in between which all keys lie.
		/// </summary>
		/// <param name = "source">The source for this extension method.</param>
		/// <returns>The interval in between which all keys lie.</returns>
		public static Interval<TKey> GetKeysInterval<TKey, TValue>( this SortedList<TKey, TValue> source )
			where TKey : IComparable<TKey>
		{
			return new Interval<TKey>( source.Keys[ 0 ], source.Keys[ source.Count - 1 ] );
		}
	}
}