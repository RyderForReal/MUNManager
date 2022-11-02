using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Config.Net;
using MUNManager.Configuration;
using MUNManager.Utils;

namespace MUNManager.Views.Setup {
	public partial class WelcomeView : UserControl {
		public WelcomeView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void EventSetup1(object? sender, RoutedEventArgs e)
		{
			MainWindow.Instance.Content = new EventSetup1();
		}

		private async void LoadConfig_Click(object? sender, RoutedEventArgs e)
		{
			var fd = new OpenFileDialog
			{
				Title = "Select a configuration file",
				AllowMultiple = false
			};
			var result = await fd.ShowAsync(MainWindow.Instance);
			if (result == null || result.Length == 0) return;

			var fsConfig = new ConfigurationBuilder<IEventConfiguration>()
				.UseIniFile(result[0])
				.Build();
			MainWindow.Instance.EventConfiguration = fsConfig;
			MainWindow.Instance.EventConfiguration.Participants = string.Join('-', fsConfig.Participants.Split('-'));
			MainWindow.Instance.EventConfiguration.Debug = IfUtils.Contains(Environment.GetCommandLineArgs(), "-d", "--debug");
			MainWindow.Instance.Content = new HomeView();
		}
	}
}