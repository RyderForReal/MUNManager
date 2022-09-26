using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MUNManager.Configuration;
// ReSharper disable InconsistentNaming

namespace MUNManager.Views {
	public partial class HomeView : UserControl {
		internal static HomeView Instance = null!;

		private readonly NumericUpDown _moderatedTimeEach;
		private readonly NumericUpDown _moderatedTime;
		private readonly NumericUpDown _unmoderatedTime;

		internal static uint ModeratedDuration;
		internal static uint ModeratedTimeEach;
		internal static uint UnmoderatedDuration;

		public HomeView()
		{
			InitializeComponent();
			Instance = this;
			var dynamicEventName = this.FindControl<TextBlock>("DynEventName");
			dynamicEventName.Text = VolatileConfiguration.EventName;

			_moderatedTimeEach = this.FindControl<NumericUpDown>("ModeratedTimePerSpeakerInput");
			_moderatedTime = this.FindControl<NumericUpDown>("ModeratedDurationInput");
			_unmoderatedTime = this.FindControl<NumericUpDown>("UnmoderatedDurationInput");
			
			var moderatedTimeEachLabel = this.FindControl<Label>("ModeratedTimePerEachLabel");
			var moderatedTimeLabel = this.FindControl<Label>("ModeratedDurationLabel");
			var unmoderatedTimeLabel = this.FindControl<Label>("UnmoderatedDurationLabel");

			moderatedTimeEachLabel.Content = $"Time per Speaker (minimum {Constants.ModeratedCaucusMinimumDurationEach})";
			moderatedTimeLabel.Content = $"Duration (minimum {Constants.ModeratedCaucusMinimumDuration})";
			unmoderatedTimeLabel.Content = $"Duration (minimum {Constants.UnmoderatedCaucusMinimumDuration})";
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
		
		private void Moderated_Click(object? sender, RoutedEventArgs e)
		{
			if (_moderatedTime.Value < Constants.ModeratedCaucusMinimumDuration || _moderatedTimeEach.Value < Constants.ModeratedCaucusMinimumDurationEach) return;
			
			ModeratedDuration = (uint)_moderatedTime.Value;
			ModeratedTimeEach = (uint)_moderatedTimeEach.Value;
			MainWindow.Instance.Content = new ModeratedCaucusView();
		}

		private void Unmoderated_Click(object? sender, RoutedEventArgs e)
		{
			if (_unmoderatedTime.Value < Constants.UnmoderatedCaucusMinimumDuration) return;
			
			UnmoderatedDuration = (uint)_unmoderatedTime.Value;
			MainWindow.Instance.Content = new UnmoderatedCaucusView();
		}
	}
}