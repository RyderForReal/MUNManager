// (c) 2022, RyderForNow. This project is licensed under AGPL v.3.0.

using System.Timers;
using Avalonia.Controls;

namespace MUNManager.Views {
	public interface ICountdownView {
		Timer Timer { get; }
		uint GlobalTimeLeft { get; set; }
		bool GlobalTimerRunning { get; set; }

		ProgressBar GlobalProgressBar { get; }

		// ReSharper disable once InconsistentNaming
		Label GlobalTimeLeft_Label { get; }

		TextBlock ViewTitle { get; }
	}

	public interface IDualCountdownView : ICountdownView {
		uint CurrentTimeLeft { get; set; }
		bool CurrentTimerRunning { get; set; }

		ProgressBar CurrentProgressBar { get; }

		// ReSharper disable once InconsistentNaming
		Label CurrentTimeLeft_Label { get; }

		Button SpeakerStartStopButton { get; }

		bool ActiveSpeaker { get; set; }
	}
}