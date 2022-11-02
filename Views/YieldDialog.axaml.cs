// (c) 2022, RyderForNow. This project is licensed under AGPL v.3.0.

using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using MUNManager.Configuration;
using MUNManager.Utils;

namespace MUNManager.Views {
	public partial class YieldDialog : Window {
		public YieldDialog()
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			Title = $"{MainWindow.Instance.EventConfiguration.EventName} | Yield remaining time";

			ModeratedCaucusView.Instance.CurrentTimerRunning = false;
			ModeratedCaucusView.Instance.GlobalTimerRunning = false;
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void NextSpeaker_Click(object? sender, RoutedEventArgs e)
		{
			var remainingTime = ModeratedCaucusView.Instance.CurrentTimeLeft;
			ModeratedCaucusView.Reset();
			ModeratedCaucusView.Instance.CurrentTimeLeft += remainingTime;
			if (MainWindow.Instance.EventConfiguration.Debug)
				Console.WriteLine($"{ModeratedCaucusView.Instance.CurrentTimeLeft} seconds remaining (added {remainingTime} seconds)");
			CountdownUtils.UpdateCountdownUI(ModeratedCaucusView.Instance, 1);
			CountdownUtils.SetCountdownUIColor(ModeratedCaucusView.Instance, Brushes.White, 1);
			Close();
		}
		
		private void Chair_Click(object? sender, RoutedEventArgs e)
		{
			ModeratedCaucusView.Reset();
			Close();
		}

		private void Cancel_Click(object? sender, RoutedEventArgs e)

		{
			ModeratedCaucusView.Instance.GlobalTimerRunning = true;
			ModeratedCaucusView.Instance.CurrentTimerRunning = true;
			Close();
		}
	}
}