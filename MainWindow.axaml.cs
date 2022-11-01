using System;
using Avalonia.Controls;
using MUNManager.Configuration;
using MUNManager.Views;
using MUNManager.Views.Setup;
using MUNManager.Utils;

namespace MUNManager
{
    public partial class MainWindow : Window
    {
        internal static MainWindow Instance;
        public MainWindow()
        {
            var args = Environment.GetCommandLineArgs();
            
            InitializeComponent();
            Instance = this;

            VolatileConfiguration.Debug = IfUtils.Contains(args, "-d", "--debug");
            Instance.Content = IfUtils.Contains(args, "-dv", "--debugView") switch
            {
                true => new DebugView()
            };
            Instance.Content = IfUtils.Contains(args, "-s", "--skipSetup") switch
            {
                true  => new HomeView(),
                false => new WelcomeView()
            };
        }
    }
}