using System.Timers;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using MUNManager.Configuration;
using MUNManager.Utils;

namespace MUNManager.Views {
	public partial class UnmoderatedCaucusView : UserControl, ICountdownView {
		internal static UnmoderatedCaucusView Instance = null!;
		public Timer Timer { get; } = new(1000);

		private readonly uint _defaultGlobalTime = HomeView.UnmoderatedDuration;

		public bool GlobalTimerRunning { get; set; }
		public uint GlobalTimeLeft { get; set; }
		public ProgressBar GlobalProgressBar { get; }
		// ReSharper disable once InconsistentNaming
		public Label GlobalTimeLeft_Label { get; }
		public TextBlock ViewTitle { get; }
		
		private readonly Button _globalButton;
		public UnmoderatedCaucusView()
		{
			InitializeComponent();
			Instance = this;
			MainWindow.Instance.Title = $"{VolatileConfiguration.EventName} | Unmoderated Caucus";

			GlobalTimeLeft = Instance._defaultGlobalTime;
			
			Timer.Elapsed += TimerOnElapsed;
			_globalButton = this.FindControl<Button>("GlobalStartStop");

			GlobalProgressBar = this.FindControl<ProgressBar>("GlobalCountdownBar");
			GlobalTimeLeft_Label = this.FindControl<Label>("GlobalTimeLeft_Label");
			GlobalProgressBar.Maximum = _defaultGlobalTime;
			GlobalProgressBar.Value = GlobalTimeLeft;
			GlobalTimeLeft_Label.Content = $"{GlobalTimeLeft}s left";
		}

		// TODO: Merge into single method to clean up this mess
		private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
		{
			if (!GlobalTimerRunning) return;
			
			GlobalTimeLeft--;
			var color = CountdownUtils.DetermineColor(GlobalTimeLeft, _defaultGlobalTime);

			if (color == VolatileConfiguration.CriticalBrush)
			{
				CountdownUtils.SetCountdownUIColor(this, color, 1);
			}
			
			if (GlobalTimeLeft == 0)
			{
				GlobalTimerRunning = false;
			}
			
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void AllStartStop_Click(object? sender, RoutedEventArgs e)
		{
			if (Timer.Enabled)
			{
				_globalButton.Content = "Start Global";
				Timer.Stop();
				GlobalCountdownBar.Foreground = Brushes.White;
				GlobalTimeLeft_Label.Foreground = Brushes.White;
				GlobalTimeLeft_Label.Content = $"{GlobalTimeLeft}s left (Paused)";	
			}
			else
			{
				Timer.Start();
				_globalButton.Content = "Pause Global";
				GlobalTimeLeft_Label.Content = $"{GlobalTimeLeft}s left";	
			}
		}

		private void Back_Click(object? sender, RoutedEventArgs e)
		{
			MainWindow.Instance.Content = new HomeView();
		}
	}
}