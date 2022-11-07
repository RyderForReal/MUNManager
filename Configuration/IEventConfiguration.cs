// (c) 2022, RyderForNow. This project is licensed under AGPL v.3.0.

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using Config.Net;

namespace MUNManager.Configuration {
	public interface IEventConfiguration {
		[Option(Alias = "General.EventName")]
		string EventName { get; set; }

		[Option(Alias = "General.DebugMode")]
		bool Debug { get; set; }
		
		// Possibly ignore this value or ask whether it should be saved. Since opening speeches are only done once, this value's only use is for template purposes.
		[Option(Alias = "OpeningSpeeches.Enabled", DefaultValue = true)]
		bool DoOpeningSpeeches { get; set; }
		[Option(Alias = "OpeningSpeeches.Duration", DefaultValue = (uint)120)]
		uint OpeningSpeechDuration { get; set; }
		
		/// <summary>
		/// Whether to hide countries that have been marked as absent in the roll call.
		/// </summary>
		[Option(Alias = "General.HideAbsent", DefaultValue = true)]
		bool HideIfAbsent { get; set; }
		
		// TODO: Change to IEnumerable<Country>.
		// IEnumerables are not supported by Config.Net, so a string needs to be used instead.
		[Option(Alias = "DataStorage.Participants")]
		
		// TODO: Currently, making a helper method to parse the IEnumerable to a string exhibits weird behavior. However, setting this value directly is not viable, so a workaround is needed.
		string Participants { get; set; } 
	}
}