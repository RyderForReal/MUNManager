using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Config.Net;
using MUNManager.Configuration;
using MUNManager.Utils;
using MUNManager.Views;
using MUNManager.Views.Setup;

namespace MUNManager {
	public partial class MainWindow : Window {
		internal static MainWindow Instance;
		public List<string> PresentParticipants = new();

		public MainWindow()
		{
			InitializeConfiguration();
			var args = Environment.GetCommandLineArgs();

			InitializeComponent();
			Instance = this;

			// Quickly show a debug view if the debug flag is set. A UserControl is used instead of a method since it has more options to play around with.
			if (IfUtils.Contains(args, "-dv", "--debugView")) Instance.Content = new DebugView();
			switch (IfUtils.Contains(args, "-s", "--skipSetup"))
			{
				case true:
					EventConfiguration.EventName = "MUNManager (Debug)";
					GlobalConfiguration.DebugMode = true;
					EventConfiguration.Participants = "Participant1-Participant2-Participant3-Participant4-Participant5-Participant6-Participant7-Participant8";
					Content = new HomeView();
					break;
				case false:
					Content = new WelcomeView();
					break;
			}
		}

		public IEventConfiguration EventConfiguration { get; internal set; } = null!;
		public IGlobalConfiguration GlobalConfiguration { get; internal set; } = null!;

		private void InitializeConfiguration()
		{
			EventConfiguration = new ConfigurationBuilder<IEventConfiguration>()
				.UseInMemoryDictionary()
				.Build();

			EventConfiguration.EventName = "MUNManager";
			EventConfiguration.DoOpeningSpeeches = true;
			EventConfiguration.HideIfAbsent = true;
			EventConfiguration.OpeningSpeechDuration = 120;

			GlobalConfiguration = new ConfigurationBuilder<IGlobalConfiguration>()
				.UseInMemoryDictionary()
				.Build();
			GlobalConfiguration.DebugMode = IfUtils.Contains(Environment.GetCommandLineArgs(), "-d", "--debug");
		}
	}
}