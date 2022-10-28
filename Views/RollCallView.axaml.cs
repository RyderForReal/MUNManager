// (c) 2022, RyderForNow. This project is licensed under AGPL v.3.0.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MUNManager.Views {
	public partial class RollCallView : UserControl {
		public RollCallView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}