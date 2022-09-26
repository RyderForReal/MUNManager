using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MUNManager.Configuration;

namespace MUNManager.Views.Setup {
	public partial class SaveDialog : Window {
		public SaveDialog()
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void ContinueVolatile(object? sender, RoutedEventArgs e)
		{
			if (VolatileConfiguration.DoOpeningSpeeches)
			{
				MainWindow.Instance.Content = new OpeningSpeechView();
			}
			else
			{
				MainWindow.Instance.Content = new HomeView();
			}
			Close();
		}
	}
}