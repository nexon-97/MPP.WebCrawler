using System;

namespace WpfClient
{
	internal class Utils
	{
		public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
		{
			var equatableValue = value as IComparable<T>;
			
			if (equatableValue.CompareTo(min) < 0)
			{
				return min;
			}
			else if (equatableValue.CompareTo(max) > 0)
			{
				return max;
			}

			return value;
		}
	}
}
