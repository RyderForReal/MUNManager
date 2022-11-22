using System.Collections.ObjectModel;
using System.Timers;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using MUNManager.Configuration;
using MUNManager.Utils;

namespace MUNManager.Views {
	public partial class ModeratedCaucusView : UserControl, IDualCountdownView {
		internal static ModeratedCaucusView Instance = null!;
		private static ObservableCollection<string> AvailableSpeakers;
		private readonly AutoCompleteBox _availableList;

		private readonly TextBlock _currentSpeaker;
		private readonly uint _defaultCurrentTime = HomeView.ModeratedTimeEach;

		private readonly uint _defaultGlobalTime = HomeView.ModeratedDuration;
		private readonly Button _globalButton;
		private readonly ListBox _nextList;
		private readonly Button _removeButton;
		private readonly Button _skipButton;
		private readonly ObservableCollection<string> _speakers = new();
		private readonly Button _yieldButton;

		public ModeratedCaucusView()
		{
			InitializeComponent();
			Instance = this;
			MainWindow.Instance.Title = $"{MainWindow.Instance.EventConfiguration.EventName} | Moderated Caucus";

			if (MainWindow.Instance.EventConfiguration.HideIfAbsent) { AvailableSpeakers = new ObservableCollection<string>(MainWindow.Instance.PresentParticipants); }
			else { AvailableSpeakers = new ObservableCollection<string>(MainWindow.Instance.EventConfiguration.Participants.Split('-')); }

			CurrentTimeLeft = _defaultCurrentTime;
			GlobalTimeLeft = _defaultGlobalTime;

			Timer.Start();
			Timer.Elapsed += TimerOnElapsed;

			SpeakerStartStopButton = this.FindControl<Button>("CurrentStartStop");
			_removeButton = this.FindControl<Button>("RemoveFromNext");
			_skipButton = this.FindControl<Button>("SkipCurrentSpeaker");
			_yieldButton = this.FindControl<Button>("YieldToNext");
			_availableList = this.FindControl<AutoCompleteBox>("AllCountries");
			ViewTitle = this.FindControl<TextBlock>("ViewTitleElement");
			_currentSpeaker = this.FindControl<TextBlock>("CurrentSpeaker");
			_nextList = this.FindControl<ListBox>("NextCountries");
			_globalButton = this.FindControl<Button>("GlobalStartStop");

			_currentSpeaker.Text = _speakers.Count.Equals(0) ? "No speakers" : _speakers[0];


			GlobalProgressBar = this.FindControl<ProgressBar>("GlobalCountdownBar");
			CurrentProgressBar = this.FindControl<ProgressBar>("CurrentCountdownBar");
			GlobalProgressBar.Maximum = _defaultGlobalTime;
			CurrentProgressBar.Maximum = _defaultCurrentTime;
			GlobalProgressBar.Value = GlobalTimeLeft;

			GlobalTimeLeft_Label = this.FindControl<Label>("GlobalCountdownText");
			CurrentTimeLeft_Label = this.FindControl<Label>("CurrentCountdownText");
			GlobalTimeLeft_Label.Content = $"{GlobalTimeLeft}s left";

			_availableList.Items = AvailableSpeakers;
			_nextList.Items = _speakers;

			this.FindControl<TextBlock>("AllAvailableSpeakersInfo").Text = $"Available: {string.Join(", ", AvailableSpeakers)}";

			CountdownUtils.SetCountdownUIColor(this, Brushes.White, 100);
		}

		public Timer Timer { get; } = new(1000);
		public uint GlobalTimeLeft { get; set; }
		public bool GlobalTimerRunning { get; set; }
		public ProgressBar GlobalProgressBar { get; }
		public Label GlobalTimeLeft_Label { get; }
		public uint CurrentTimeLeft { get; set; }
		public bool CurrentTimerRunning { get; set; }
		public ProgressBar CurrentProgressBar { get; }
		public Label CurrentTimeLeft_Label { get; }
		public bool ActiveSpeaker { get; set; }
		public TextBlock ViewTitle { get; }
		public Button SpeakerStartStopButton { get; }

		private void UpdateSpeaker()
		{
			if (ActiveSpeaker)
				return;
			_currentSpeaker.Text = _speakers[0];
			_speakers.RemoveAt(0);

			// TODO: Disable "remove from list" button if no entries
			_skipButton.IsEnabled = _speakers.Count != 0;
			_yieldButton.IsEnabled = _speakers.Count != 0;

			ActiveSpeaker = true;
		}

		private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
		{
			if (GlobalTimerRunning)
			{
				if (GlobalTimeLeft > 0)
					GlobalTimeLeft--;
				CountdownUtils.UpdateCountdownUI(this, 2, GlobalTimeLeft == 0);
				var color = CountdownUtils.DetermineColor(GlobalTimeLeft, _defaultGlobalTime);
				CountdownUtils.SetCountdownUIColor(this, color, 2, Equals(color, VolatileConfiguration.CriticalBrush));
			}

			// ReSharper disable once InvertIf
			if (CurrentTimerRunning)
			{
				if (CurrentTimeLeft > 0)
					CurrentTimeLeft--;
				CountdownUtils.UpdateCountdownUI(this, 1, CurrentTimeLeft == 0);
				var color = CountdownUtils.DetermineColor(CurrentTimeLeft, _defaultCurrentTime);
				CountdownUtils.SetCountdownUIColor(this, color, 1, Equals(color, VolatileConfiguration.CriticalBrush));
				if (CurrentTimeLeft != 0)
					return;
				ActiveSpeaker = false;
			}
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void CurrentStartStop_onClick(object? sender, RoutedEventArgs e)
		{
			ActiveSpeaker = true;
			if (CurrentTimerRunning)
			{
				CurrentTimerRunning = false;
				SpeakerStartStopButton.Content = "Start Current";
				CountdownUtils.UpdateCountdownUI(this, 1, false, true);
			}
			else
			{
				CurrentTimerRunning = true;
				GlobalTimerRunning = true;
				// TODO: Implement buttons in the Interface and use parameter to determine the text (start/stop).
				SpeakerStartStopButton.Content = "Pause Current";
				_globalButton.Content = "Pause Global";
				CountdownUtils.UpdateCountdownUI(this, 100);
			}
		}

		internal static void Reset()
		{
			Instance.ActiveSpeaker = false;
			Instance.CurrentTimeLeft = Instance._defaultCurrentTime;
			Dispatcher.UIThread.Post(() =>
			{
				Instance.SpeakerStartStopButton.Content = "Start";
				Instance.CurrentTimerRunning = false;
				Instance.CurrentProgressBar.Value = Instance._defaultCurrentTime;
			});
			if (Instance._speakers.Count == 0)
			{
				Dispatcher.UIThread.Post(() =>
				{
					Instance._currentSpeaker.Text = "No speakers";
					Instance.SpeakerStartStopButton.IsEnabled = false;
					Instance._skipButton.IsEnabled = false;
					Instance._yieldButton.IsEnabled = false;
				});
			}
			else { Instance.UpdateSpeaker(); }
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
				SpeakerStartStopButton.Content = "Start Current";
				CurrentTimerRunning = false;
				GlobalTimerRunning = false;
				CountdownUtils.SetCountdownUIColor(this, Brushes.White, 100);
				GlobalTimeLeft_Label.Content = $"{GlobalTimeLeft}s left (Paused)";
			}
			else
			{
				_globalButton.Content = "Pause Global";
				if (!_currentSpeaker.Text.Equals("No speakers"))
				{
					CurrentTimerRunning = true;
					SpeakerStartStopButton.Content = "Pause Current";
				}

				GlobalTimerRunning = true;
				CountdownUtils.UpdateCountdownUI(this, 100);
			}
		}

		private void AddToNextUp(object? sender, RoutedEventArgs e)
		{
			if (_availableList.SelectedItem == null) return;
			_speakers.Add(_availableList.SelectedItem.ToString());
			UpdateSpeaker();
			switch (_speakers.Count)
			{
				case 0:
					Dispatcher.UIThread.Post(() => { SpeakerStartStopButton.IsEnabled = true; });
					break;
				case >= 1:
					Dispatcher.UIThread.Post(() =>
					{
						_skipButton.IsEnabled = true;
						_yieldButton.IsEnabled = true;
					});
					break;
			}
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