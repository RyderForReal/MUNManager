using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Config.Net;
using MUNManager.Configuration;

namespace MUNManager.Views.Setup {
	public partial class SaveDialog : Window {
		private static SaveDialog _instance;

		public SaveDialog()
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			_instance = this;
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void ContinueVolatile(object? sender, RoutedEventArgs e)
		{
			MainWindow.Instance.Content = new RollCallView();
			Close();
		}

		private async void Save_Click(object? sender, RoutedEventArgs e)
		{
			var fd = new SaveFileDialog
			{
				Title = "Select a configuration file",
				DefaultExtension = "ini",
				InitialFileName = "EventConfiguration.ini"
			};
			var result = await fd.ShowAsync(_instance);
			if (result == null) return;

			var fsConfig = new ConfigurationBuilder<IEventConfiguration>()
				.UseIniFile(result)
				.Build();
			//fsConfig.Debug = MainWindow.Instance.GlobalConfiguration.DebugMode;
			fsConfig.Participants = string.Join('-', MainWindow.Instance.EventConfiguration.Participants.Split('-'));
			fsConfig.EventName = MainWindow.Instance.EventConfiguration.EventName.Replace(';', '-');
			fsConfig.DoOpeningSpeeches = MainWindow.Instance.EventConfiguration.DoOpeningSpeeches;
			fsConfig.HideIfAbsent = MainWindow.Instance.EventConfiguration.HideIfAbsent;
			fsConfig.OpeningSpeechDuration = MainWindow.Instance.EventConfiguration.OpeningSpeechDuration;
			MainWindow.Instance.Content = new HomeView();
			Close();
		}
	}
}