using System;
using Avalonia.Media;

namespace MUNManager.Configuration {
	[Obsolete("This class is in the process of being replaced.")]
	public static class VolatileConfiguration {
		// WIP Implementation
		// ReSharper disable once InconsistentNaming
		public static IBrush OKBrush { get; set; } = new SolidColorBrush(Colors.Green);
		public static IBrush PreWarningBrush { get; set; } = new SolidColorBrush(Colors.YellowGreen);
		public static IBrush WarningBrush { get; set; } = new SolidColorBrush(Colors.Yellow);
		public static IBrush AlertBrush { get; set; } = new SolidColorBrush(Colors.Orange);
		public static IBrush CriticalBrush { get; set; } = new SolidColorBrush(Colors.Red);
		public static IBrush ErrorBrush { get; set; } = new SolidColorBrush(Colors.DarkRed);

		public static double PreWarningThreshold { get; set; } = 0.25;
		public static double WarningThreshold { get; set; } = 0.2;
		public static double AlertThreshold { get; set; } = 0.1;
		public static double CriticalThreshold { get; set; } = 0.05;
	}
}