using System.Collections.ObjectModel;
using System.Timers;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using MUNManager.Configuration;
using MUNManager.Utils;
using Timer = System.Timers.Timer;

namespace MUNManager.Views {
	public partial class ModeratedCaucusView : UserControl, IDualCountdownView {

		internal static ModeratedCaucusView Instance = null!;
		private readonly ObservableCollection<string> _speakers = new();
		private static readonly ObservableCollection<string> AvailableSpeakers = new(VolatileConfiguration.Participants ?? new() { "No participants" });

		public Timer Timer { get; } = new(1000);
		public uint GlobalTimeLeft { get; set; }
		public bool GlobalTimerRunning { get; set; }
		public ProgressBar GlobalProgressBar { get; }
		public Label GlobalTimeLeft_Label { get; }
		public uint CurrentTimeLeft { get; set; }
		public bool CurrentTimerRunning { get; set; }
		public ProgressBar CurrentProgressBar { get; }
		public Label CurrentTimeLeft_Label { get; }

		private readonly uint _defaultGlobalTime = HomeView.ModeratedDuration;
		private readonly uint _defaultCurrentTime = HomeView.ModeratedTimeEach;

		private readonly TextBlock _currentSpeaker;
		public TextBlock ViewTitle { get; }
		private readonly ListBox _nextList;
		private readonly ListBox _availableList;
		private readonly Button _globalButton;
		private readonly Button _currentButton;
		private readonly Button _skipButton;
		private readonly Button _yieldButton;

		public ModeratedCaucusView()
		{
			InitializeComponent();
			Instance = this;
			MainWindow.Instance.Title = $"{VolatileConfiguration.EventName} | Moderated Caucus";

			CurrentTimeLeft = _defaultCurrentTime;
			GlobalTimeLeft = _defaultGlobalTime;

			Timer.Start();
			Timer.Elapsed += TimerOnElapsed;

			_currentButton = this.FindControl<Button>("CurrentStartStop");
			_skipButton = this.FindControl<Button>("SkipCurrentSpeaker");
			_yieldButton = this.FindControl<Button>("YieldToNext");
			_availableList = this.FindControl<ListBox>("AllCountries");
			ViewTitle = this.FindControl<TextBlock>("ViewTitleElement");
			_currentSpeaker = this.FindControl<TextBlock>("CurrentSpeaker");
			_nextList = this.FindControl<ListBox>("NextCountries");
			_globalButton = this.FindControl<Button>("GlobalStartStop");

			_currentSpeaker.Text = _speakers.Count.Equals(0) ? "No speakers" : _speakers[0];

			GlobalProgressBar = this.FindControl<ProgressBar>("GlobalCountdownBar");
			CurrentProgressBar = this.FindControl<ProgressBar>("CurrentCountdownBar");
			GlobalTimeLeft_Label = this.FindControl<Label>("GlobalCountdownText");
			CurrentTimeLeft_Label = this.FindControl<Label>("CurrentCountdownText");
			GlobalProgressBar.Maximum = _defaultGlobalTime;
			CurrentProgressBar.Maximum = _defaultCurrentTime;
			GlobalProgressBar.Value = GlobalTimeLeft;
			GlobalTimeLeft_Label.Content = $"{GlobalTimeLeft}s left";

			// TODO: Remove item at index 0 for UI (Added to Git, remove soon)
			_availableList.Items = AvailableSpeakers;
			_nextList.Items = _speakers;
		}

		private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
		{
			if (GlobalTimerRunning)
			{
				if (GlobalTimeLeft > 0)
					GlobalTimeLeft--;
				CountdownUtils.UpdateCountdownUI(this, 1);
				var color = CountdownUtils.DetermineColor(GlobalTimeLeft, _defaultGlobalTime);
				CountdownUtils.SetCountdownUIColor(this, CountdownUtils.DetermineColor(GlobalTimeLeft, _defaultGlobalTime), 1, Equals(color, VolatileConfiguration.CriticalBrush));
			}

			// ReSharper disable once InvertIf
			if (CurrentTimerRunning)
			{
				if (CurrentTimeLeft > 0)
					CurrentTimeLeft--;
				CountdownUtils.UpdateCountdownUI(this, 2);
				var color = CountdownUtils.DetermineColor(CurrentTimeLeft, _defaultCurrentTime);
				CountdownUtils.SetCountdownUIColor(this, CountdownUtils.DetermineColor(CurrentTimeLeft, _defaultCurrentTime), 2, Equals(color, VolatileConfiguration.CriticalBrush));
			}
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void CurrentStartStop_onClick(object? sender, RoutedEventArgs e)
		{
			if (_speakers.Count == 0 && !VolatileConfiguration.Debug) return;

			if (CurrentTimerRunning)
			{
				CurrentTimerRunning = false;
				_currentButton.Content = "Start Current";
			}
			else
			{
				CurrentTimerRunning = true;
				GlobalTimerRunning = true;
				_currentButton.Content = "Pause Current";
				_globalButton.Content = "Pause Global";

			}
		}

		internal static void Reset()
		{
			Dispatcher.UIThread.Post(() =>
			{
				Instance._currentButton.Content = "Start";
				Instance.CurrentTimeLeft = Instance._defaultCurrentTime;
				Instance.CurrentCountdownBar.Value = Instance._defaultCurrentTime;
				Instance._currentSpeaker.Text = Instance._speakers.Count.Equals(0) ? "No speakers" : Instance._speakers[0];
			});
			if (Instance._speakers.Count == 0)
			{
				Dispatcher.UIThread.Post(() =>
				{
					Instance._currentButton.IsEnabled = false;
					Instance._skipButton.IsEnabled = false;
					Instance._yieldButton.IsEnabled = false;
				});
				return;
			}

			Instance._speakers.RemoveAt(0);
		}

		private void SkipCurrentSpeaker_Click(object? sender, RoutedEventArgs e)
		{
			Reset();
		}

		private void AllStartStop_Click(object? sender, RoutedEventArgs e)
		{
			if (GlobalTimerRunning)
			{
				_globalButton.Content = "Start Global";
				_currentButton.Content = "Start Current";
				CurrentTimerRunning = false;
				GlobalTimerRunning = false;
				GlobalProgressBar.Foreground = Brushes.White;
				GlobalCountdownText.Foreground = Brushes.White;
				CurrentProgressBar.Foreground = Brushes.White;
				CurrentCountdownText.Foreground = Brushes.White;
				GlobalCountdownText.Content = $"{GlobalTimeLeft}s left (Paused)";
			}
			else
			{
				_globalButton.Content = "Pause Global";
				if (!_currentSpeaker.Text.Equals("No speakers"))
				{
					CurrentTimerRunning = true;
					_currentButton.Content = "Pause Current";
					CurrentCountdownText.Content = $"{CurrentTimeLeft}s left";
				}

				GlobalTimerRunning = true;
				GlobalCountdownText.Content = $"{GlobalTimeLeft}s left";
			}
		}

		private void AddToNextUp(object? sender, RoutedEventArgs e)
		{
			if (_availableList.SelectedItems.Count == 0) return;
			if (_speakers.Count == 0)
			{
				Dispatcher.UIThread.Post(() =>
				{
					_currentButton.IsEnabled = true;
					_skipButton.IsEnabled = true;
					_yieldButton.IsEnabled = true;
				});
			}

			_speakers.Add(_availableList.SelectedItems[0].ToString());

			if (_speakers.Count == 1) { _currentSpeaker.Text = _speakers[0]; }
		}

		private void Remove_Click(object? sender, RoutedEventArgs e)
		{
			if (_nextList.SelectedItems.Count == 0) return;
			_speakers.Remove(_nextList.SelectedItems[0].ToString());
		}

		private void Back_Click(object? sender, RoutedEventArgs e)
		{
			MainWindow.Instance.Content = new HomeView();
		}

		private void Yield_Click(object? sender, RoutedEventArgs e)
		{
			if (_speakers.Count == 0) return;
			new YieldDialog().Show();
		}
	}
}