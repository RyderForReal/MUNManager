using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Timers;
using Avalonia;
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

		private readonly Timer _timer = new(1000);
		internal bool GlobalTimerRunning { get; set; }
		internal bool CurrentTimerRunning { get; set; }
		
		private readonly uint _defaultGlobalTime = HomeView.ModeratedDuration;
		private readonly uint _defaultCurrentTime = HomeView.ModeratedTimeEach;
		internal uint CurrentTimeLeft;
		private uint _globalTimeLeft;

		private readonly ProgressBar _globalCountdownBar;
		private readonly ProgressBar _currentCountdownBar;
		private readonly Label _globalCountdownText;
		private readonly Label _currentCountdownText;
		private readonly TextBlock _currentSpeaker;
		private readonly TextBlock _viewTitle;
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
			_globalTimeLeft = _defaultGlobalTime;
			
			//GlobalTimer.Elapsed += GlobalTimerOnElapsed;
			//CurrentTimer.Elapsed += CurrentTimerOnElapsed;

			_timer.Start();
			_timer.Elapsed += TimerOnElapsed;

			_currentButton = this.FindControl<Button>("CurrentStartStop");
			_skipButton = this.FindControl<Button>("SkipCurrentSpeaker");
			_yieldButton = this.FindControl<Button>("YieldToNext");
			_availableList = this.FindControl<ListBox>("AllCountries");
			_viewTitle = this.FindControl<TextBlock>("ViewTitle");
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
			_availableList.Items = AvailableSpeakers;
			_nextList.Items = _speakers;
		}
		
		private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
		{
			if (GlobalTimerRunning)
			{
				if (_globalTimeLeft > 0)
					_globalTimeLeft--;
				UpdateCountdownUI(1);
				var color = CountdownUtils.DetermineColor(_globalTimeLeft, _defaultGlobalTime);
				SetCountdownUIColor(CountdownUtils.DetermineColor(_globalTimeLeft, _defaultGlobalTime), 1, Equals(color, VolatileConfiguration.CriticalBrush));
			}
			// ReSharper disable once InvertIf
			if (CurrentTimerRunning)
			{
				if (CurrentTimeLeft > 0)
					CurrentTimeLeft--;
				UpdateCountdownUI(2);
				var color = CountdownUtils.DetermineColor(CurrentTimeLeft, _defaultCurrentTime);
				SetCountdownUIColor(CountdownUtils.DetermineColor(CurrentTimeLeft, _defaultCurrentTime), 2, Equals(color, VolatileConfiguration.CriticalBrush));
			}
		}

		// ReSharper disable once InconsistentNaming
		private void UpdateCountdownUI(uint mode, bool expired = false)
		{
			void Update()
			{
				switch (mode)
				{
					case 1:
						_globalCountdownBar.Value = _globalTimeLeft;
						_globalCountdownText.Content = !expired ? $"{_globalTimeLeft}s left" : $"The caucus has ended ({_globalTimeLeft}s left).";
						break;
					case 2:
						_currentCountdownBar.Value = CurrentTimeLeft;
						_currentCountdownText.Content = !expired ? $"{CurrentTimeLeft}s left" : $"The speaker's time has run out ({CurrentTimeLeft}s left).";
						break;
					default:
						_globalCountdownBar.Value = _globalTimeLeft;
						_globalCountdownText.Content = !expired ? $"{_globalTimeLeft}s left" : $"The caucus has ended ({_globalTimeLeft}s left).";
						_currentCountdownBar.Value = CurrentTimeLeft;
						_currentCountdownText.Content = !expired ? $"{CurrentTimeLeft}s left" : $"The speaker's time has run out ({CurrentTimeLeft}s left).";
						break;
				}	
			}
			Dispatcher.UIThread.Post(Update);
		}
		
		// ReSharper disable once InconsistentNaming
		/// <param name="mode">1: GlobalTimer, 2: CurrentTimer, else update both</param>
		/// <param name="isAlert">Whether to color the view's title</param>
		// ReSharper disable once InvalidXmlDocComment
		private void SetCountdownUIColor(IBrush color, uint mode, bool isAlert = false)
		{
			void Update()
			{
				switch (mode)
				{
					case 1:
						_globalCountdownBar.Foreground = color;
						_globalCountdownText.Foreground = color;
						if (isAlert) { _viewTitle.Foreground = color; }

						break;
					case 2:
						_currentCountdownBar.Foreground = color;
						_currentCountdownText.Foreground = color;
						break;
					default:
						_currentCountdownBar.Foreground = color;
						_currentCountdownText.Foreground = color;
						_globalCountdownBar.Foreground = color;
						_globalCountdownText.Foreground = color;
						if (isAlert) { _viewTitle.Foreground = color; }

						break;
				}
			}
			Dispatcher.UIThread.Post(Update);
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
			} else {
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
			if (GlobalTimerRunning)
			{
				_globalButton.Content = "Start Global";
				_currentButton.Content = "Start Current";
				CurrentTimerRunning = false;
				GlobalTimerRunning = false;
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
					CurrentTimerRunning	= true;
					_currentButton.Content = "Pause Current";
					_currentCountdownText.Content = $"{CurrentTimeLeft}s left";
				}
				GlobalTimerRunning = true;
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