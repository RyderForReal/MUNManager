using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Config.Net;
using MUNManager.Configuration;
using MUNManager.Utils;

namespace MUNManager.Views.Setup {
	public partial class WelcomeView : UserControl {
		public WelcomeView()
		{
			InitializeComponent();
			MainWindow.Instance.Background = Brushes.Transparent;
			MainWindow.Instance.TransparencyLevelHint = WindowTransparencyLevel.AcrylicBlur;

			this.FindControl<Button>("PreferencesButton").IsVisible = MainWindow.Instance.GlobalConfiguration.DebugMode;
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
			// Check whether it needs to be split and re-joined - seems odd.
			MainWindow.Instance.EventConfiguration.Participants = string.Join('-', fsConfig.Participants.Split('-'));
			//MainWindow.Instance.GlobalConfiguration.Debug = IfUtils.Contains(Environment.GetCommandLineArgs(), "-d", "--debug");
			MainWindow.Instance.Content = new HomeView();
		}

		private void Preferences_Click(object? sender, RoutedEventArgs e)
		{
			new PreferenceWindow().Show();
		}
	}
}