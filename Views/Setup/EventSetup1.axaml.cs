using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using MUNManager.Configuration;

namespace MUNManager.Views.Setup {
	public partial class EventSetup1 : UserControl {
		private static ToggleSwitch _doOpeningSpeeches = null!;
		public EventSetup1()
		{
			InitializeComponent();
			MainWindow.Instance.Background = new SolidColorBrush(Color.Parse("#121212"));
			MainWindow.Instance.TransparencyLevelHint = WindowTransparencyLevel.None;
			_doOpeningSpeeches = this.Find<ToggleSwitch>("OpeningSpeeches");
			MainWindow.Instance.Title = "Set up an Event | MUNManager";
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void NextView(object? sender, RoutedEventArgs e)
		{
			var eventName = this.FindControl<TextBox>("EventName");

			if (string.IsNullOrEmpty(eventName.Text))
			{
				eventName.Text = "MUNManager";
			}
			MainWindow.Instance.EventConfiguration.EventName = eventName.Text;
			if (MainWindow.Instance.EventConfiguration.EventName != "MUNManager")
			{
				MainWindow.Instance.Title = $"{MainWindow.Instance.EventConfiguration.EventName} | MUNManager";
			}
			MainWindow.Instance.EventConfiguration.DoOpeningSpeeches = _doOpeningSpeeches.IsChecked!.Value;
			MainWindow.Instance.EventConfiguration.OpeningSpeechDuration = (uint)this.FindControl<NumericUpDown>("OpeningSpeechDuration").Value;

			MainWindow.Instance.Content = new CountrySelection();
		}

		private void WelcomeView(object? sender, RoutedEventArgs e)
		{
			MainWindow.Instance.Content = new WelcomeView();
		}
	}
}