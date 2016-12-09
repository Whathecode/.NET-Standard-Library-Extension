﻿using System;
using System.Collections.Generic;
using Whathecode.System.Algorithm;


namespace Whathecode.System
{
	/// <summary>
	/// Interface specifying an interval from a value, to a value. Borders may be included or excluded.
	/// </summary>
    public interface IInterval<T> : IInterval<T, T>
		where T : IComparable<T>
    {
		/// <summary>
		/// Limit a given range to this range.
		/// When part of the given range lies outside of this range, it isn't included in the resulting range.
		/// </summary>
		/// <param name = "range">The range to limit to this range.</param>
		/// <returns>The given range, which excludes all parts lying outside of this range. Null when empty.</returns>
		IInterval<T> Clamp( IInterval<T> range );

		/// <summary>
		/// Split the interval into two intervals at the given point, or nearest valid point.
		/// </summary>
		/// <param name = "atPoint">The point where to split.</param>
		/// <param name = "option">Option which specifies in which intervals the split point ends up.</param>
		/// <param name = "before">The interval in which to store the part before the point, if any, null otherwise.</param>
		/// <param name = "after">The interval in which to store the part after the point, if any, null otherwise.</param>
		void Split( T atPoint, SplitOption option, out IInterval<T> before, out IInterval<T> after );

		/// <summary>
		/// Subtract a given interval from the current interval.
		/// </summary>
		/// <param name = "subtract">The interval to subtract from this interval.</param>
		/// <returns>The resulting intervals after subtraction.</returns>
		List<IInterval<T>> Subtract( IInterval<T> subtract );

		/// <summary>
		/// Returns the intersection of this interval with another.
		/// </summary>
		/// <param name = "interval">The interval to get the intersection for.</param>
		/// <returns>The intersection of this interval with the given other. Null when no intersection.</returns>
		IInterval<T> Intersection( IInterval<T> interval );

		/// <summary>
		/// Returns an expanded interval of the current interval up to the given value (and including).
		/// When the value lies within the interval the returned interval is the same.
		/// </summary>
		/// <param name = "value">The value up to which to expand the interval.</param>
		new IInterval<T> ExpandTo( T value );

		/// <summary>
		/// Returns an expanded interval of the current interval up to the given value.
		/// When the value lies within the interval the returned interval is the same.
		/// </summary>
		/// <param name = "value">The value up to which to expand the interval.</param>
		/// <param name = "include">Include the value to which is expanded in the interval.</param>
		new IInterval<T> ExpandTo( T value, bool include );

		/// <summary>
		/// Returns an interval offsetted from the current interval by a specified amount.
		/// </summary>
		/// <param name="amount">How much to move the interval.</param>
		new IInterval<T> Move( T amount );

		/// <summary>
		/// Returns a scaled version of the current interval.
		/// </summary>
		/// <param name="scale">
		/// Percentage to scale the interval up or down.
		/// Smaller than 1.0 to scale down, larger to scale up.
		/// </param>
		/// <param name="aroundPercentage">The percentage inside the interval around which to scale.</param>
		new IInterval<T> Scale( double scale, double aroundPercentage = 0.5 );

		/// <summary>
		/// Returns a reversed version of the current interval, swapping the start position with the end position.
		/// </summary>
		new IInterval<T> Reverse();
    }

	public interface IInterval<T, TSize>
		where T : IComparable<T>
		where TSize : IComparable<TSize>
	{
		/// <summary>
		/// The start of the interval.
		/// </summary>
		T Start { get; }

		/// <summary>
		/// The end of the interval.
		/// </summary>
		T End { get; }

		/// <summary>
		/// Is the value at the start of the interval included in the interval.
		/// </summary>
		bool IsStartIncluded { get; }

		/// <summary>
		/// Is the value at the end of the interval included in the interval.
		/// </summary>
		bool IsEndIncluded { get; }

		/// <summary>
		/// Determines whether the start of the interval lies before or after the end of the interval. true when after, false when before.
		/// </summary>
		bool IsReversed { get; }

		/// <summary>
		/// Get the value in the center of the interval. Rounded to the nearest correct value.
		/// </summary>
		T Center { get; }

		/// <summary>
		/// Get the size of the interval.
		/// </summary>
		TSize Size { get; }


		#region Get operations.

		/// <summary>
		/// Get the value at a given percentage within (0.0 - 1.0) or outside (&lt; 0.0, &gt; 1.0) of the interval. Rounding to nearest neighbour occurs when needed.
		/// TODO: Would it be cleaner not to use a double for percentage, but a generic Percentage type?
		/// </summary>
		/// <param name = "percentage">The percentage in the range of which to return the value.</param>
		/// <returns>The value at the given percentage within the interval.</returns>
		T GetValueAt( double percentage );

		/// <summary>
		/// Get a percentage how far inside (0.0 - 1.0) or outside (&lt; 0.0, &gt; 1.0) the interval a certain value lies.
		/// For single intervals, '1.0' is returned when inside the interval, '-1.0' otherwise.
		/// </summary>
		/// <param name = "position">The position value to get the percentage for.</param>
		/// <returns>The percentage indicating how far inside (or outside) the interval the given value lies.</returns>
		double GetPercentageFor( T position );

		/// <summary>
		/// Map a value from this range, to a value in another range linearly.
		/// </summary>
		/// <param name = "value">The value to map to another range.</param>
		/// <param name = "range">The range to which to map the value.</param>
		/// <returns>The value, mapped to the given range.</returns>
		T Map( T value, IInterval<T, TSize> range );

		/// <summary>
		/// Map a value from this range, to a value in another range of another type linearly.
		/// </summary>
		/// <typeparam name = "TOther">The type of the other range.</typeparam>
		/// <typeparam name = "TOtherSize">The type used to specify distances in between two values of <typeparamref name="TOther" />.</typeparam>
		/// <param name = "value">The value to map to another range.</param>
		/// <param name = "range">The range to which to map the value.</param>
		/// <returns>The value, mapped to the given range.</returns>
		TOther Map<TOther, TOtherSize>( T value, IInterval<TOther, TOtherSize> range )
			where TOther : IComparable<TOther>
			where TOtherSize : IComparable<TOtherSize>;

		/// <summary>
		/// Does the given value lie in the interval or not.
		/// </summary>
		/// <param name = "value">The value to check for.</param>
		/// <returns>True when the value lies within the interval, false otherwise.</returns>
		bool LiesInInterval( T value );

		/// <summary>
		/// Does the given interval intersect the other interval.
		/// </summary>
		/// <param name = "interval">The interval to check for intersection.</param>
		/// <returns>True when the intervals intersect, false otherwise.</returns>
		bool Intersects( IInterval<T, TSize> interval );

		/// <summary>
		/// Limit a given value to this range. When the value is smaller/bigger than the range, snap it to the range border.
		/// TODO: For now this does not take into account whether the start or end of the range is included. Is this possible?
		/// </summary>
		/// <param name = "value">The value to limit.</param>
		/// <returns>The value limited to the range.</returns>
		T Clamp( T value );

		/// <summary>
		/// Limit a given range to this range.
		/// When part of the given range lies outside of this range, it isn't included in the resulting range.
		/// </summary>
		/// <param name = "range">The range to limit to this range.</param>
		/// <returns>The given range, which excludes all parts lying outside of this range. Null when empty.</returns>
		IInterval<T, TSize> Clamp( IInterval<T, TSize> range );

		/// <summary>
		/// Split the interval into two intervals at the given point, or nearest valid point.
		/// </summary>
		/// <param name = "atPoint">The point where to split.</param>
		/// <param name = "option">Option which specifies in which intervals the split point ends up.</param>
		/// <param name = "before">The interval in which to store the part before the point, if any, null otherwise.</param>
		/// <param name = "after">The interval in which to store the part after the point, if any, null otherwise.</param>
		void Split( T atPoint, SplitOption option, out IInterval<T, TSize> before, out IInterval<T, TSize> after );

		/// <summary>
		/// Subtract a given interval from the current interval.
		/// </summary>
		/// <param name = "subtract">The interval to subtract from this interval.</param>
		/// <returns>The resulting intervals after subtraction.</returns>
		List<IInterval<T, TSize>> Subtract( IInterval<T, TSize> subtract );

		/// <summary>
		/// Returns the intersection of this interval with another.
		/// </summary>
		/// <param name = "interval">The interval to get the intersection for.</param>
		/// <returns>The intersection of this interval with the given other. Null when no intersection.</returns>
		IInterval<T, TSize> Intersection( IInterval<T, TSize> interval );

		#endregion  // Get operations.


		#region Enumeration

		/// <summary>
		/// Get values for each step within the interval.
		/// </summary>
		/// <param name="step">The step size between each value.</param>
		IEnumerable<T> GetValues( TSize step );

		/// <summary>
		/// Get values for each step within the interval, anchored to multiples of a specified anchor value.
		/// </summary>
		/// <param name="step">The step size between each value.</param>
		/// <param name="anchor">The value to which multiples of step are anchored.</param>
		IEnumerable<T> GetValues( TSize step, T anchor );

		/// <summary>
		/// Execute an action each step in an interval.
		/// </summary>
		/// <param name = "step">The size of the steps.</param>
		/// <param name = "stepAction">The operation to execute.</param>
		void EveryStepOf( TSize step, Action<T> stepAction );

		#endregion  // Enumeration


		#region Modifiers

		/// <summary>
		/// Returns an expanded interval of the current interval up to the given value (and including).
		/// When the value lies within the interval the returned interval is the same.
		/// </summary>
		/// <param name = "value">The value up to which to expand the interval.</param>
		IInterval<T, TSize> ExpandTo( T value );

		/// <summary>
		/// Returns an expanded interval of the current interval up to the given value.
		/// When the value lies within the interval the returned interval is the same.
		/// </summary>
		/// <param name = "value">The value up to which to expand the interval.</param>
		/// <param name = "include">Include the value to which is expanded in the interval.</param>
		IInterval<T, TSize> ExpandTo( T value, bool include );

		/// <summary>
		/// Returns an interval offsetted from the current interval by a specified amount.
		/// </summary>
		/// <param name="amount">How much to move the interval.</param>
		IInterval<T, TSize> Move( TSize amount );

		/// <summary>
		/// Returns a scaled version of the current interval.
		/// </summary>
		/// <param name="scale">
		/// Percentage to scale the interval up or down.
		/// Smaller than 1.0 to scale down, larger to scale up.
		/// </param>
		/// <param name="aroundPercentage">The percentage inside the interval around which to scale.</param>
		IInterval<T, TSize> Scale( double scale, double aroundPercentage = 0.5 );

		/// <summary>
		/// Returns a scaled version of the current interval, but prevents the interval from exceeding the values specified in a passed limit.
		/// This is useful to prevent <see cref="ArgumentOutOfRangeException" /> during calculations for certain types.
		/// </summary>
		/// <param name="scale">
		/// Percentage to scale the interval up or down.
		/// Smaller than 1.0 to scale down, larger to scale up.
		/// </param>
		/// <param name="limit">The limit which the interval snaps to when scaling exceeds it.</param>
		/// <param name="aroundPercentage">The percentage inside the interval around which to scale.</param>
		IInterval<T, TSize> Scale( double scale, IInterval<T, TSize> limit, double aroundPercentage = 0.5 );

		/// <summary>
		/// Returns a reversed version of the current interval, swapping the start position with the end position.
		/// </summary>
		IInterval<T, TSize> Reverse();

		#endregion  // Modifiers


		object Clone();
	}
}
