// (c) 2022, RyderForNow. This project is licensed under AGPL v.3.0.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MUNManager.Configuration;

namespace MUNManager.Views {
	public partial class YieldDialog : Window {
		internal static YieldDialog Instance = null!;

		public YieldDialog()
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			Instance = this;
			Title = $"{VolatileConfiguration.EventName} | Yielding";
			
			ModeratedCaucusView.Instance._currentTimer.Stop();
			ModeratedCaucusView.Instance._globalTimer.Stop();
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
			Close();
		}
		
		private void Chair_Click(object? sender, RoutedEventArgs e)
		{
			ModeratedCaucusView.Reset();
			Close();
		}

		private void Cancel_Click(object? sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}