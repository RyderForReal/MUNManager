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
	public partial class ModeratedCaucusView : UserControl {

		internal static ModeratedCaucusView Instance = null!;
		private readonly ObservableCollection<string> _speakers = new();
		private static readonly ObservableCollection<string> AvailableSpeakers = new(VolatileConfiguration.Participants ?? new() { "No participants" });
		internal readonly Timer _currentTimer = new(1000);
		internal readonly Timer _globalTimer = new(1000);

		private readonly uint _defaultGlobalTime = HomeView.ModeratedDuration;
		private readonly uint _defaultCurrentTime = HomeView.ModeratedTimeEach;
		internal uint CurrentTimeLeft;
		private uint _globalTimeLeft;

		private ProgressBar _globalCountdownBar = null!;
		private ProgressBar _currentCountdownBar = null!;
		private Label _globalCountdownText = null!;
		private Label _currentCountdownText = null!;
		private ListBox _availableList = null!;
		private TextBlock _currentSpeaker = null!;
		private ListBox _nextList = null!;
		private TextBlock _viewTitle = null!;
		private Button _globalButton = null!;
		private Button _currentButton = null!;
		private Button _skipButton = null!;
		private Button _yieldButton = null!;
		public ModeratedCaucusView()
		{
			InitializeComponent();
			Instance = this;
			MainWindow.Instance.Title = VolatileConfiguration.EventName + " | Moderated Caucus";

			CurrentTimeLeft = _defaultCurrentTime;
			_globalTimeLeft = _defaultGlobalTime;
			
			_globalTimer.Elapsed += GlobalTimerOnElapsed;
			_currentTimer.Elapsed += CurrentTimerOnElapsed;

			_currentButton = this.FindControl<Button>("CurrentStartStop");
			_skipButton = this.FindControl<Button>("SkipCurrentSpeaker");
			_yieldButton = this.FindControl<Button>("YieldToNext");
			_availableList = this.FindControl<ListBox>("AllCountries");
			_viewTitle = this.FindControl<TextBlock>("ViewTitle");
			_availableList.Items = AvailableSpeakers;
			_currentSpeaker = this.FindControl<TextBlock>("CurrentSpeaker");
			_nextList = this.FindControl<ListBox>("NextCountries");
			_globalButton = this.FindControl<Button>("GlobalStartStop");

			_currentSpeaker.Text = _speakers.Count.Equals(0) ? "No speakers" : _speakers[0];

			_globalCountdownBar = this.FindControl<ProgressBar>("GlobalCountdownBar");
			_currentCountdownBar = this.FindControl<ProgressBar>("CurrentCountdownBar");
			_globalCountdownText = this.FindControl<Label>("GlobalCountdownText");
			_currentCountdownText = this.FindControl<Label>("CurrentCountdownText");
			_globalCountdownBar.Maximum = _defaultGlobalTime;
			_currentCountdownBar.Maximum = _defaultCurrentTime;
			_globalCountdownBar.Value = _globalTimeLeft;
			_globalCountdownText.Content = $"{_globalTimeLeft}s left";

			// TODO: Remove item at index 0 for UI (Added to Git, remove soon)
			_nextList.Items = _speakers;
		}
		

		// TODO: Merge into single method to clean up this mess
		private void GlobalTimerOnElapsed(object? sender, ElapsedEventArgs e)
		{
			_globalTimeLeft--;
			Dispatcher.UIThread.Post(() =>
			{
				_globalCountdownBar.Value = _globalTimeLeft;
				_globalCountdownText.Content = $"{_globalTimeLeft}s left";

				if (_globalTimeLeft > _defaultGlobalTime * 0.25)
				{
					_globalCountdownBar.Foreground = Brushes.Green;
					_globalCountdownText.Foreground = Brushes.Green;
				} else if (IfUtils.IsWithinBounds(_globalTimeLeft, _defaultGlobalTime, 0.2 ,0.25))
				{
					_globalCountdownBar.Foreground = Brushes.YellowGreen;
					_globalCountdownText.Foreground = Brushes.YellowGreen;
				} else if (IfUtils.IsWithinBounds(_globalTimeLeft, _defaultGlobalTime, 0.15, 0.2))
				{
					_globalCountdownBar.Foreground = Brushes.Yellow;
					_globalCountdownText.Foreground = Brushes.Yellow;
				}  else if (IfUtils.IsWithinBounds(_globalTimeLeft, _defaultGlobalTime, 0.05, 0.1))
				{
					_globalCountdownBar.Foreground = Brushes.Orange;
					_globalCountdownText.Foreground = Brushes.Orange;
				} else if (_globalTimeLeft < _defaultGlobalTime * 0.05)
				{
					_globalCountdownBar.Foreground = Brushes.Red;
					_globalCountdownText.Foreground = Brushes.Red;
					_viewTitle.Foreground = Brushes.Red;
				}

				if (_globalTimeLeft != 0)
					return;
				_globalCountdownBar.Background = Brushes.Red;
				_globalCountdownText.Content = "This caucus has ended (0s left).";
				_currentCountdownBar.Background = Brushes.Red;
				_globalTimer.Stop();
				_currentTimer.Stop();
			});
		}

		private void CurrentTimerOnElapsed(object? sender, ElapsedEventArgs e)
		{
			CurrentTimeLeft--;
			Dispatcher.UIThread.Post(() =>
			{
				_currentCountdownBar.Value = CurrentTimeLeft;
				_currentCountdownText.Content = $"{CurrentTimeLeft}s left";

				if (CurrentTimeLeft > _defaultCurrentTime * 0.25)
				{
					_currentCountdownBar.Foreground = Brushes.Green;
					_currentCountdownText.Foreground = Brushes.Green;
				} else if (IfUtils.IsWithinBounds(CurrentTimeLeft, _defaultGlobalTime, 0.2 ,0.25))
				{
					_currentCountdownBar.Foreground = Brushes.YellowGreen;
					_currentCountdownText.Foreground = Brushes.YellowGreen;
				} else if (IfUtils.IsWithinBounds(CurrentTimeLeft, _defaultGlobalTime, 0.15, 0.2))
				{
					_currentCountdownBar.Foreground = Brushes.Yellow;
					_currentCountdownText.Foreground = Brushes.Yellow;
				}  else if (IfUtils.IsWithinBounds(CurrentTimeLeft, _defaultGlobalTime, 0.05, 0.1))
				{
					_currentCountdownBar.Foreground = Brushes.Orange;
					_currentCountdownText.Foreground = Brushes.Orange;
				} else if (CurrentTimeLeft < _defaultCurrentTime * 0.05)
				{
					_currentCountdownBar.Foreground = Brushes.Red;
					_currentCountdownText.Foreground = Brushes.Red;
					_currentSpeaker.Foreground = Brushes.Red;
				}
			});

			if (CurrentTimeLeft > 0)
				return;
			_currentTimer.Stop();
			Reset();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void CurrentStartStop_onClick(object? sender, RoutedEventArgs e)
		{
			if (_speakers.Count == 0 && !VolatileConfiguration.Debug) return;

			if (_currentTimer.Enabled)
			{
				_currentTimer.Stop();
				_currentButton.Content = "Start Current";
			} else {
				_globalTimer.Start();
				_currentTimer.Start();
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
				Instance._currentCountdownBar.Value = Instance._defaultCurrentTime;
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
			if (_globalTimer.Enabled)
			{
				_globalButton.Content = "Start Global";
				_currentButton.Content = "Start Current";
				_currentTimer.Stop();
				_globalTimer.Stop();
				_globalCountdownBar.Foreground = Brushes.White;
				_globalCountdownText.Foreground = Brushes.White;
				_currentCountdownBar.Foreground = Brushes.White;
				_currentCountdownText.Foreground = Brushes.White;
				_globalCountdownText.Content = $"{_globalTimeLeft}s left (Paused)";	
			}
			else
			{
				_globalButton.Content = "Pause Global";
				if (!_currentSpeaker.Text.Equals("No speakers"))
				{
					_currentTimer.Start();
					_currentButton.Content = "Pause Current";
					_currentCountdownText.Content = $"{CurrentTimeLeft}s left";
				}
				_globalTimer.Start();
				_globalCountdownText.Content = $"{_globalTimeLeft}s left";	
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
			
			if (_speakers.Count == 1)
			{
				_currentSpeaker.Text = _speakers[0];
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