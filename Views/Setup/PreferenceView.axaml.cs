// (c) 2022, RyderForNow. This project is licensed under AGPL v.3.0.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MUNManager.Views.Setup {
	public partial class PreferenceView : UserControl {
		public PreferenceView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void Save_Click(object? sender, RoutedEventArgs e)
		{
			throw new System.NotImplementedException();
		}
	}
}