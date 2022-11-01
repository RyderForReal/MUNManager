// (c) 2022, RyderForNow. This project is licensed under AGPL v.3.0.

// This class is for testing purposes only.
using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MUNManager.Views {
	public partial class DebugView : UserControl {
		public DebugView()
		{
			InitializeComponent();
			Console.WriteLine("DebugView initialized");
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}