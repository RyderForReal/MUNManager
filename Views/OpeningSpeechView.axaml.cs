using System.Collections.Generic;
using System.Timers;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;

namespace MUNManager.Views {
	public partial class OpeningSpeechView : UserControl {
		private static ProgressBar _progressBar = null!;
		private static TextBlock _progressText = null!;

		private static readonly Timer Timer = new(1000);
		private static uint _timeLeft = MainWindow.Instance.EventConfiguration.OpeningSpeechDuration;
		private static readonly uint TimeDefault = MainWindow.Instance.EventConfiguration.OpeningSpeechDuration;

		private static string _currentCountry = string.Empty;
		private static List<string> NextUp;

		private static Button _startStop = null!;
		private static TextBlock _currentCountryText = null!;

		public OpeningSpeechView()
		{
			InitializeComponent();
			MainWindow.Instance.Title = $"{MainWindow.Instance.EventConfiguration.EventName} | Opening Speeches";

			if (MainWindow.Instance.EventConfiguration.HideIfAbsent) { NextUp = new List<string>(MainWindow.Instance.PresentParticipants); }
			else { NextUp = new List<string>(MainWindow.Instance.EventConfiguration.Participants.Split('-')); }

			_currentCountry = NextUp[0];
			_currentCountryText = this.FindControl<TextBlock>("CurrentlySpeakingName");
			_currentCountryText.Text = _currentCountry;

			Timer.Elapsed += timer_Tick;

			_progressBar = this.FindControl<ProgressBar>("Duration");
			_progressText = this.FindControl<TextBlock>("DurationText");
			_startStop = this.FindControl<Button>("StartStop");
			this.FindControl<Button>("Skip");

			_progressBar.Maximum = TimeDefault;
			_progressBar.Value = TimeDefault;

			_progressText.Text = $"{TimeDefault}s";

			_progressBar.Foreground = Brushes.White;
			_progressText.Foreground = Brushes.White;
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void StartPauseEvent(object? sender, RoutedEventArgs e)
		{
			if (Timer.Enabled)
			{
				Timer.Stop();
				_startStop.Content = "Start";
			}
			else
			{
				Timer.Start();
				_startStop.Content = "Pause";
			}
		}

		private static void timer_Tick(object? sender, ElapsedEventArgs elapsedEventArgs)
		{
			_timeLeft--;
			if (_timeLeft <= 0) { Timer.Stop(); }

			// TODO: Optimization - Determine which if statement to use before executing main thread code, then use switch statement
			// TODO: Delay color changes to a bit later, they're too early
			Dispatcher.UIThread.Post(() =>
			{
				_progressBar.Value = _timeLeft;
				_progressText.Text = $"{_timeLeft}s left";

				if (_timeLeft > TimeDefault * 0.25)
				{
					_progressBar.Foreground = Brushes.Green;
					_progressText.Foreground = Brushes.Green;
				}
				else if (_timeLeft < TimeDefault * 0.25 && _timeLeft > TimeDefault * 0.20)
				{
					_progressBar.Foreground = Brushes.YellowGreen;
					_progressText.Foreground = Brushes.YellowGreen;
				}
				else if (_timeLeft < TimeDefault * 0.20 && _timeLeft > TimeDefault * 0.15)
				{
					_progressBar.Foreground = Brushes.Yellow;
					_progressText.Foreground = Brushes.Yellow;
				}
				else if (_timeLeft < TimeDefault * 0.1 && _timeLeft > TimeDefault * 0.05)
				{
					_progressBar.Foreground = Brushes.Orange;
					_progressText.Foreground = Brushes.Orange;
				}
				else if (_timeLeft < TimeDefault * 0.05)
				{
					_progressBar.Foreground = Brushes.Red;
					_progressText.Foreground = Brushes.Red;
				}

			});
		}

		private void SkipEvent(object? sender, RoutedEventArgs e)
		{
			if (NextUp.Count > 1)
			{
				NextUp.RemoveAt(0);
				Reset();
			}
			else { MainWindow.Instance.Content = new HomeView(); }
		}

		// TODO: Add reset functionality w/o skipping country
		private static void Reset()
		{
			if (Timer.Enabled) Timer.Stop();
			_currentCountry = NextUp[0];
			_currentCountryText.Text = _currentCountry;
			_progressBar.Foreground = Brushes.White;
			_progressText.Foreground = Brushes.White;
			_startStop.Content = "Start";

			_timeLeft = TimeDefault;
			_progressText.Text = $"{TimeDefault}s left";
			_progressBar.Value = TimeDefault;
		}
	}
}