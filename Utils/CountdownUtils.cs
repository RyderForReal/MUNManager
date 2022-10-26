// (c) 2022, RyderForNow. This project is licensed under AGPL v.3.0.

using Avalonia.Media;
using Avalonia.Threading;
using MUNManager.Configuration;
using MUNManager.Views;

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
		// ReSharper disable once InconsistentNaming
		public static void UpdateCountdownUI<T>(T countdownView, int mode, bool expired = false)
			where T : IDualCountdownView, ICountdownView
		{
			void Update()
			{
				switch (mode)
				{
					case 1:
						countdownView.GlobalProgressBar.Value = countdownView.GlobalTimeLeft;
						countdownView.GlobalTimeLeft_Label.Content = !expired ? $"{countdownView.GlobalTimeLeft}s left" : $"The caucus has ended ({countdownView.GlobalTimeLeft}s left).";
						break;
					case 2:
						countdownView.CurrentProgressBar.Value = countdownView.CurrentTimeLeft;
						countdownView.CurrentTimeLeft_Label.Content = !expired ? $"{countdownView.CurrentTimeLeft}s left" : $"The speaker's time has run out ({countdownView.CurrentTimeLeft}s left).";
						break;
					default:
						countdownView.GlobalProgressBar.Value = countdownView.GlobalTimeLeft;
						countdownView.GlobalTimeLeft_Label.Content = !expired ? $"{countdownView.GlobalTimeLeft}s left" : $"The caucus has ended ({countdownView.GlobalTimeLeft}s left).";
						countdownView.CurrentProgressBar.Value = countdownView.CurrentTimeLeft;
						countdownView.CurrentTimeLeft_Label.Content = !expired ? $"{countdownView.CurrentTimeLeft}s left" : $"The speaker's time has run out ({countdownView.CurrentTimeLeft}s left).";
						break;
				}
			}
			Dispatcher.UIThread.Post(Update);
		}
		// ReSharper disable once InconsistentNaming
		/// <param name="mode">1: GlobalTimer, 2: CurrentTimer, else update both</param>
		/// <param name="isAlert">Whether to color the view's title</param>
		// ReSharper disable once InvalidXmlDocComment
		public static void SetCountdownUIColor<T>(T countdownView, IBrush color, uint mode, bool isAlert = false)
		where T : IDualCountdownView, ICountdownView
		{
			void Update()
			{
				switch (mode)
				{
					case 1:
						countdownView.GlobalProgressBar.Foreground = color;
						countdownView.GlobalTimeLeft_Label.Foreground = color;
						if (isAlert) { countdownView.ViewTitle.Foreground = color; }

						break;
					case 2:
						countdownView.CurrentProgressBar.Foreground = color;
						countdownView.CurrentTimeLeft_Label.Foreground = color;
						break;
					default:
						countdownView.CurrentProgressBar.Foreground = color;
						countdownView.CurrentTimeLeft_Label.Foreground = color;
						countdownView.GlobalProgressBar.Foreground = color;
						countdownView.GlobalTimeLeft_Label.Foreground = color;
						if (isAlert) { countdownView.ViewTitle.Foreground = color; }
						break;
				}
			}

			Dispatcher.UIThread.Post(Update);
		}
	}
}