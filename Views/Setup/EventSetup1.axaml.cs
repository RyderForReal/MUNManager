using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MUNManager.Configuration;

namespace MUNManager.Views.Setup {
	public partial class EventSetup1 : UserControl {
		private static ToggleSwitch _doOpeningSpeeches = null!;
		public EventSetup1()
		{
			InitializeComponent();
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
			VolatileConfiguration.EventName = eventName.Text;
			if (VolatileConfiguration.EventName != "MUNManager")
			{
				MainWindow.Instance.Title = $"{VolatileConfiguration.EventName} | MUNManager";
			}
			
			VolatileConfiguration.DoOpeningSpeeches = _doOpeningSpeeches.IsChecked!.Value;
			VolatileConfiguration.OpeningSpeechDuration = (uint)this.FindControl<NumericUpDown>("OpeningSpeechDuration").Value;
			//VolatileConfiguration.SpeechDuration = (uint)this.FindControl<NumericUpDown>("SpeechDuration").Value;
			//VolatileConfiguration.CaucusDuration = (uint)this.FindControl<NumericUpDown>("CaucusDuration").Value;
			
			MainWindow.Instance.Content = new CountrySelection();
		}

		private void WelcomeView(object? sender, RoutedEventArgs e)
		{
			MainWindow.Instance.Content = new WelcomeView();
		}
	}
}