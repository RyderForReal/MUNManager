// (c) 2022, RyderForNow. This project is licensed under AGPL v.3.0.

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Config.Net;

namespace MUNManager.Configuration {
	public interface IEventConfiguration {
		[Option(Alias = "General.EventName")]
		string EventName { get; set; }
		
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
		[Option(Alias = "Data.Participants")]
		
		// IEnumerables are not supported by Config.Net, so a string needs to be used instead.
		string Participants { get; set; }

		void SetParticipants(IEnumerable<string> countries)
		{
			Participants = string.Join("\n", countries);
		}
		
		// Library requires 1 parameter, clean up this mess later.
		IEnumerable<string> GetParticipants(bool ignored = false)
		{
			return Participants.Split('\n');
		}
	}
}