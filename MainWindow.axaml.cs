using System;
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
        internal static MainWindow Instance;
        public MainWindow()
        {
            InitializeConfiguration();
            var args = Environment.GetCommandLineArgs();
            
            InitializeComponent();
            Instance = this;

            // Quickly show a debug view if the debug flag is set. A UserControl is used instead of a method since it has more options to play around with.
            if(IfUtils.Contains(args, "-dv", "--debugView")) Instance.Content = new DebugView();
                Instance.Content = IfUtils.Contains(args, "-s", "--skipSetup") switch
            {
                true  => new HomeView(),
                false => new WelcomeView()
            };
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