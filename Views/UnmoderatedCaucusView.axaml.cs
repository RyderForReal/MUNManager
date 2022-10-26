using System.Timers;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using MUNManager.Configuration;
using MUNManager.Utils;

namespace MUNManager.Views {
	public partial class UnmoderatedCaucusView : UserControl {
		internal static UnmoderatedCaucusView Instance = null!;
		private readonly Timer _timer = new(1000);

		private readonly uint _defaultGlobalTime = HomeView.UnmoderatedDuration;
		private uint _globalTimeLeft;

		private readonly ProgressBar _globalCountdownBar;
		private readonly Label _globalCountdownText;
		private readonly Button _globalButton;
		public UnmoderatedCaucusView()
		{
			InitializeComponent();
			Instance = this;
			MainWindow.Instance.Title = $"{VolatileConfiguration.EventName} | Unmoderated Caucus";

			_globalTimeLeft = Instance._defaultGlobalTime;
			
			_timer.Elapsed += TimerOnElapsed;
			_globalButton = this.FindControl<Button>("GlobalStartStop");

			_globalCountdownBar = this.FindControl<ProgressBar>("GlobalCountdownBar");
			_globalCountdownText = this.FindControl<Label>("GlobalCountdownText");
			_globalCountdownBar.Maximum = _defaultGlobalTime;
			_globalCountdownBar.Value = _globalTimeLeft;
			_globalCountdownText.Content = $"{_globalTimeLeft}s left";
		}

		// TODO: Merge into single method to clean up this mess
		private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
		{
			_globalTimeLeft--;
			if (_globalTimeLeft == 0)
			{
				_timer.Stop();
			}
			Dispatcher.UIThread.Post(() =>
			{
				_globalCountdownBar.Value = _globalTimeLeft;
				_globalCountdownText.Content = $"{_globalTimeLeft}s left";
				if (_globalTimeLeft > _defaultGlobalTime * 0.25)
				{
					_globalCountdownBar.Foreground = Brushes.Green;
					_globalCountdownText.Foreground = Brushes.Green;
				} else if (IfUtils.IsWithinBounds((int)_globalTimeLeft, (int)_defaultGlobalTime, 0.2, 0.25))
				{
					_globalCountdownBar.Foreground = Brushes.YellowGreen;
					_globalCountdownText.Foreground = Brushes.YellowGreen;
				} else if (IfUtils.IsWithinBounds((int)_globalTimeLeft, (int)_defaultGlobalTime, 0.15, 0.2))
				{
					_globalCountdownBar.Foreground = Brushes.Yellow;
					_globalCountdownText.Foreground = Brushes.Yellow;
				}  else if (IfUtils.IsWithinBounds(_globalTimeLeft, _defaultGlobalTime, 0.1, 0.05))
				{
					_globalCountdownBar.Foreground = Brushes.Orange;
					_globalCountdownText.Foreground = Brushes.Orange;
				} else if (_globalTimeLeft < _defaultGlobalTime * 0.05)
				{
					_globalCountdownBar.Foreground = Brushes.Red;
					_globalCountdownText.Foreground = Brushes.Red;
				}

			});
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void AllStartStop_Click(object? sender, RoutedEventArgs e)
		{
			if (_timer.Enabled)
			{
				_globalButton.Content = "Start Global";
				_timer.Stop();
				_globalCountdownBar.Foreground = Brushes.White;
				_globalCountdownText.Foreground = Brushes.White;
				_globalCountdownText.Content = $"{_globalTimeLeft}s left (Paused)";	
			}
			else
			{
				_timer.Start();
				_globalButton.Content = "Pause Global";
				_globalCountdownText.Content = $"{_globalTimeLeft}s left";	
			}
		}

		private void Back_Click(object? sender, RoutedEventArgs e)
		{
			MainWindow.Instance.Content = new HomeView();
		}
	}
}