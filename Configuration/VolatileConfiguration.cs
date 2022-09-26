using System.Collections;
using System.Collections.Generic;

namespace MUNManager.Configuration {
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
	}
}