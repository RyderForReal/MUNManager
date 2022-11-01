using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia.Media;

namespace MUNManager.Configuration {
	[Obsolete("This class will be deprecated in favor of Config.NET's memory configuration provider soon.")]
	public static class VolatileConfiguration {
		public static string EventName { get; set; } = "MUNManager";
		public static List<string>? Participants { get; set; }

		public static uint OpeningSpeechDuration { get; set; } = 120;
		
		// TODO: Implement SpeechDuration
		public static uint SpeechDuration { get; set; } = 60;
		public static uint CaucusDuration { get; set; } = 240;

		public static bool DoOpeningSpeeches { get; set; } = true;
		public static bool Debug { get; set;  } = false;
		public static List<string> SpeechTypes { get; } = new() { "Moderated Caucus", "Unmoderated Caucus" };
		
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