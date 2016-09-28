﻿using System;
using Whathecode.System.Algorithm;


namespace Whathecode.System.Diagnostics
{
	/// <summary>
	/// Class which measures the runtime of a given action.
	/// </summary>
	/// <remarks>
	/// Useful when used as follows:
	/// var measure = RunTime.From( "Time measure", () =>
	/// {
	///   ... statements to execute ...
	/// } );
	/// </remarks>
	public class RunTime
	{
		/// <summary>
		/// Get the time measurement from a given action.
		/// </summary>
		/// <param name = "action">The action to measure the time from.</param>
		/// <returns>The time measurement of the action.</returns>
		public static StatisticsStopwatch.Measurement From( Action action )
		{
			if ( action == null )
			{
				throw new ArgumentNullException( nameof( action ) );
			}

			return From( "", action );
		}

		/// <summary>
		/// Get the time measurement from a given action.
		/// </summary>
		/// <param name = "label">The label to indicate which measurement it is upon calling ToString() on the result.</param>
		/// <param name = "action">The action to measure the time from.</param>
		/// <returns>The time measurement of the action.</returns>
		public static StatisticsStopwatch.Measurement From( string label, Action action )
		{
			if ( action == null )
			{
				throw new ArgumentNullException( nameof( action ) );
			}

			return From( label, action, 1 );
		}

		/// <summary>
		/// Get the time measurement of a certain action executed an amount of times.
		/// </summary>
		/// <param name = "label">The label to indicate which measurement it is upon calling ToString().</param>
		/// <param name = "action">The action to measure the time from.</param>
		/// <param name = "times">The amount of times the action should run.</param>
		/// <returns>The time measurement of the executed actions.</returns>
		public static StatisticsStopwatch.Measurement From( string label, Action action, int times )
		{
			if ( action == null )
			{
				throw new ArgumentNullException( nameof( action ) );
			}
			if ( times < 0 )
			{
				throw new ArgumentOutOfRangeException( nameof( times ), "Should be greater than zero." );
			}

			StatisticsStopwatch stopwatch = null;
			Loop loop = Loop.NumberOfTimes( times, action );

			// Start a new measurement every iteration.
			loop.Before.First( () => stopwatch = StatisticsStopwatch.Start( label ) );
			loop.After.AllButLast( () => stopwatch.StartNextMeasurement() );
			loop.Run();

			return stopwatch.Stop();
		}
	}
}