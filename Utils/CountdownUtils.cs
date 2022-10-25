// (c) 2022, RyderForNow. This project is licensed under AGPL v.3.0.

using Avalonia.Media;
using MUNManager.Configuration;

namespace MUNManager.Utils {
	public class CountdownUtils {
		public static IBrush DetermineColor(uint value, uint compareTo)
		{
			if (value > compareTo * VolatileConfiguration.PreWarningThreshold)
			{
				return VolatileConfiguration.OKBrush;
			}

			if (IfUtils.IsWithinBounds(value, compareTo, VolatileConfiguration.WarningThreshold ,VolatileConfiguration.PreWarningThreshold))
			{
				return VolatileConfiguration.PreWarningBrush;
			}

			if (IfUtils.IsWithinBounds(value, compareTo, VolatileConfiguration.AlertThreshold, VolatileConfiguration.WarningThreshold))
			{
				return VolatileConfiguration.WarningBrush;
			}

			if (IfUtils.IsWithinBounds(value, compareTo, VolatileConfiguration.CriticalThreshold, VolatileConfiguration.AlertThreshold))
			{
				return VolatileConfiguration.AlertBrush;
			}
			return VolatileConfiguration.CriticalBrush;
		}
	}
}