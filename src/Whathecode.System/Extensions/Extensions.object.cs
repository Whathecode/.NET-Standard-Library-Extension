﻿using System;
using System.Linq;
using System.Reflection;


namespace Whathecode.System.Extensions
{
	public partial class Extensions
	{
		/// <summary>
		/// Determines whether the reference of the specified <see cref = "object" /> equals that from this object
		/// when it's a reference type, or whether the values equal when it's a boxed value.
		/// </summary>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "o">The object to compare with.</param>
		public static bool ReferenceOrBoxedValueEquals( this object source, object o )
		{
			return ((o != null) && o.GetType().GetTypeInfo().IsValueType)
				? source.Equals( o )
				: source == o;
		}

		/// <summary>
		/// Returns whether the object equals any of the given values.
		/// </summary>
		/// <typeparam name = "T">Type of the objects to compare.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "toCompare">The objects to compare with.</param>
		/// <returns>True when the object equals any of the passed objects, false otherwise.</returns>
		public static bool EqualsAny<T>( this T source, params T[] toCompare )
		{
			return toCompare.Any( o => o.Equals( source ) );
		}

		/// <summary>
		/// Returns whether all given values equal the object.
		/// </summary>
		/// <typeparam name="T">Type of the objects to compare.</typeparam>
		/// <param name="source">The source for this extension method.</param>
		/// <param name="toCompare">The objects to compare with.</param>
		/// <returns>True when all given values equal the object, false otherwise.</returns>
		public static bool EqualsAll<T>( this T source, params T[] toCompare )
		{
			return toCompare.All( o => o.Equals( source ) );
		}

		/// <summary>
		/// Returns a selected value when the source is not null; null otherwise.
		/// </summary>
		/// <typeparam name = "T">Type of the source object.</typeparam>
		/// <typeparam name = "TInner">Type of the object which the selector returns.</typeparam>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "selector">A function which given the source object, returns a selected value.</param>
		/// <returns>The selected value when source is not null; null otherwise.</returns>
		public static TInner IfNotNull<T, TInner>( this T source, Func<T, TInner> selector )
			where T : class
		{
			return source != null ? selector( source ) : default( TInner );
		}
	}
}