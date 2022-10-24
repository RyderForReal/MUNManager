using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAvalonia.Core;

namespace MUNManager.Utils {
	public static class IfUtils {
		public static bool Contains(IEnumerable list, params string[] values)
		{
			return values.Any(list.Contains);
		}
		public static bool IsWithinBounds(int itemToCheck, int min, int max)
        {
            return itemToCheck >= min && itemToCheck <= max;
        }
		
		/// <summary>
		/// Example: a <= bx && a >= by, where a = itemToCheck, b = checkAgainst, x/y = max/min
		/// </summary>
		/// <param name="itemToCheck"></param>
		/// <param name="checkAgainst"></param>
		/// <param name="lowerBound">Percentage in decimal form</param>
		/// <param name="upperBound">Percentage in decimal form</param>
		/// <returns></returns>
        public static bool IsWithinBounds(double itemToCheck, double checkAgainst, double lowerBound, double upperBound)
		{ return itemToCheck <= checkAgainst * upperBound && itemToCheck >= checkAgainst * lowerBound; }

	}
}