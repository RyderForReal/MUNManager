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
			Timer.Start();
			MainWindow.Instance.Title = $"{MainWindow.Instance.EventConfiguration.EventName} | Unmoderated Caucus";

			GlobalTimeLeft = Instance._defaultGlobalTime;
			
			Timer.Elapsed += TimerOnElapsed;
			_globalButton = this.FindControl<Button>("GlobalStartStop");

			GlobalProgressBar = this.FindControl<ProgressBar>("GlobalCountdownBar");
			GlobalTimeLeft_Label = this.FindControl<Label>("GlobalCountdownText");
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
			
			CountdownUtils.SetCountdownUIColor(this, color, Equals(color, VolatileConfiguration.CriticalBrush));

			CountdownUtils.UpdateCountdownUI(this, GlobalTimeLeft == 0);
			
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
			if (GlobalTimerRunning)
			{
				_globalButton.Content = "Start Global";
				GlobalTimeLeft_Label.Content = $"{GlobalTimeLeft}s left (Paused)";	

				GlobalTimerRunning = false;
				
				CountdownUtils.SetCountdownUIColor(this, Brushes.White);
			}
			else
			{
				GlobalTimerRunning = true;
				
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