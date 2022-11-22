// (c) 2022, RyderForNow. This project is licensed under AGPL v.3.0.

using Avalonia.Media;
using Avalonia.Threading;
using MUNManager.Configuration;
using MUNManager.Views;

namespace MUNManager.Utils {
	public static class CountdownUtils {
		public static IBrush DetermineColor(uint value, uint compareTo)
		{
			if (value > compareTo * VolatileConfiguration.PreWarningThreshold) { return VolatileConfiguration.OKBrush; }

			if (IfUtils.IsWithinBounds(value, compareTo, VolatileConfiguration.WarningThreshold, VolatileConfiguration.PreWarningThreshold)) { return VolatileConfiguration.PreWarningBrush; }

			if (IfUtils.IsWithinBounds(value, compareTo, VolatileConfiguration.AlertThreshold, VolatileConfiguration.WarningThreshold)) { return VolatileConfiguration.WarningBrush; }

			// ReSharper disable once ConvertIfStatementToReturnStatement
			if (IfUtils.IsWithinBounds(value, compareTo, VolatileConfiguration.CriticalThreshold, VolatileConfiguration.AlertThreshold)) { return VolatileConfiguration.AlertBrush; }

			return VolatileConfiguration.CriticalBrush;
		}

		/// <summary>
		/// </summary>
		/// <param name="countdownView">The countdown view to operate on.</param>
		/// <param name="mode">1: Speaker, 2: Global, else both</param>
		/// <param name="expired">Whether the time has run out.</param>
		/// <param name="paused">Whether the timer is paused.</param>
		// ReSharper disable once InconsistentNaming
		public static void UpdateCountdownUI(IDualCountdownView countdownView, int mode, bool expired = false, bool paused = false)
		{
			switch (mode)
			{
				case 1:
					Dispatcher.UIThread.Post(UpdateCurrent);
					break;
				case 2:
					UpdateCountdownUI(countdownView, expired, paused);
					break;
				default:
					Dispatcher.UIThread.Post(UpdateCurrent);
					UpdateCountdownUI(countdownView, expired, paused);
					break;
			}

			void UpdateCurrent()
			{
				if (paused)
				{
					countdownView.CurrentTimeLeft_Label.Content = $"{countdownView.CurrentTimeLeft}s left (paused)";
					SetCountdownUIColor(countdownView, Brushes.White, 1);
				}
				else if (expired) { countdownView.CurrentTimeLeft_Label.Content = $"The speaker's time has run out ({countdownView.CurrentTimeLeft}s left)."; }
				else { countdownView.CurrentTimeLeft_Label.Content = $"{countdownView.CurrentTimeLeft}s left"; }

				countdownView.CurrentProgressBar.Value = countdownView.CurrentTimeLeft;
			}
		}

		// ReSharper disable once InconsistentNaming
		public static void UpdateCountdownUI(ICountdownView countdownView, bool expired = false, bool paused = false)
		{
			void Update()
			{
				if (paused)
				{
					countdownView.GlobalTimeLeft_Label.Content = $"{countdownView.GlobalTimeLeft}s left (paused)";
					SetCountdownUIColor(countdownView, Brushes.White);
				}
				else if (expired) { countdownView.GlobalTimeLeft_Label.Content = $"The caucus has ended ({countdownView.GlobalTimeLeft}s left)."; }
				else { countdownView.GlobalTimeLeft_Label.Content = $"{countdownView.GlobalTimeLeft}s left"; }

				countdownView.GlobalProgressBar.Value = countdownView.GlobalTimeLeft;
			}

			Dispatcher.UIThread.Post(Update);
		}

		// ReSharper disable once InconsistentNaming
		/// <param name="color">The color to theme the UI with.</param>
		/// <param name="mode">1: Speaker, 2: Global, else update both</param>
		/// <param name="isAlert">Whether to color the view's title</param>
		// ReSharper disable once InvalidXmlDocComment
		public static void SetCountdownUIColor(IDualCountdownView countdownView, IBrush color, uint mode, bool isAlert = false)
		{
			switch (mode)
			{
				case 1:
					Dispatcher.UIThread.Post(UpdateCurrent);
					break;
				case 2:
					SetCountdownUIColor(countdownView, color, isAlert);
					break;
				default:
					SetCountdownUIColor(countdownView, color, isAlert);
					Dispatcher.UIThread.Post(UpdateCurrent);
					break;
			}

			void UpdateCurrent()
			{
				countdownView.CurrentProgressBar.Foreground = color;
				countdownView.CurrentTimeLeft_Label.Foreground = color;
			}
		}

		// ReSharper disable once InconsistentNaming
		public static void SetCountdownUIColor(ICountdownView countdownView, IBrush color, bool isAlert = false)
		{
			void UpdateGlobal()
			{
				countdownView.GlobalProgressBar.Foreground = color;
				countdownView.GlobalTimeLeft_Label.Foreground = color;
				if (isAlert) { countdownView.ViewTitle.Foreground = color; }
			}

			Dispatcher.UIThread.Post(UpdateGlobal);
		}
	}
}