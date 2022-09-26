using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

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
	}
}