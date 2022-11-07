using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Config.Net;
using MUNManager.Configuration;
using MUNManager.Views;
using MUNManager.Views.Setup;
using MUNManager.Utils;

namespace MUNManager
{
    public partial class MainWindow : Window
    {
        public IEventConfiguration EventConfiguration { get; internal set; } = null!;
        public List<string> PresentParticipants = new();
        internal static MainWindow Instance;
        public MainWindow()
        {
            InitializeConfiguration();
            var args = Environment.GetCommandLineArgs();
            
            InitializeComponent();
            Instance = this;

            // Quickly show a debug view if the debug flag is set. A UserControl is used instead of a method since it has more options to play around with.
            if(IfUtils.Contains(args, "-dv", "--debugView")) Instance.Content = new DebugView();
            switch (IfUtils.Contains(args, "-s", "--skipSetup"))
            {
                case true:
                    EventConfiguration.EventName = "MUNManager (Debug)";
                    EventConfiguration.Debug = true;
                    EventConfiguration.Participants = "Participant1-Participant2-Participant3-Participant4-Participant5-Participant6-Participant7-Participant8";
                    Content = new HomeView();
                    break;
                case false:
                    Content = new WelcomeView();
                    break;
            }
        }

        private void InitializeConfiguration()
        {
            EventConfiguration = new ConfigurationBuilder<IEventConfiguration>()
                .UseInMemoryDictionary()
                .Build();
            
            EventConfiguration.EventName = "MUNManager";
            EventConfiguration.Debug = IfUtils.Contains(Environment.GetCommandLineArgs(), "-d", "--debug");
            EventConfiguration.DoOpeningSpeeches = true;
            EventConfiguration.HideIfAbsent = true;
            EventConfiguration.OpeningSpeechDuration = 120;
        }
    }
}