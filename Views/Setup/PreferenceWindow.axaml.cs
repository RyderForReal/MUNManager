// (c) 2022, RyderForNow. This project is licensed under AGPL v.3.0.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MUNManager.Views.Setup {
	public partial class PreferenceWindow : Window {
		public PreferenceWindow()
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			Content = new PreferenceView();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}