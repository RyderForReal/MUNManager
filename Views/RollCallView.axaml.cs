// (c) 2022, RyderForNow. This project is licensed under AGPL v.3.0.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MUNManager.Views {
	public partial class RollCallView : UserControl {
		private readonly ObservableCollection<string> _absentCountries = new(MainWindow.Instance.EventConfiguration.Participants.Split('-'));
		private readonly ObservableCollection<string> _presentCountries = new();
		
		private readonly ListBox _absentListBox;
		private readonly ListBox _presentListBox;
		public RollCallView()
		{
			InitializeComponent();
			_absentListBox = this.FindControl<ListBox>("AbsentCountries");
			_presentListBox = this.FindControl<ListBox>("PresentCountries");
			_absentListBox.Items = _absentCountries;
			_presentListBox.Items = _presentCountries;
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void Present_Click(object? sender, RoutedEventArgs e)
		{
			if (_absentListBox.SelectedItems.Count == 0) return;
			_presentCountries.Add(_absentListBox.SelectedItems[0].ToString());
			_absentCountries.Remove(_absentListBox.SelectedItems[0].ToString());
		}
		private void Absent_Click(object? sender, RoutedEventArgs e)
		{
			if (_presentListBox.SelectedItems.Count == 0) return;
			_absentCountries.Add(_presentListBox.SelectedItems[0].ToString());
			_presentCountries.Remove(_presentListBox.SelectedItems[0].ToString());
		}

		private void RollCallFinished_Click(object? sender, RoutedEventArgs e)
		{
			foreach (var country in _presentCountries)
			{
				var c = country.Insert(0, "@");
				MainWindow.Instance.EventConfiguration.Participants.Replace(country, c);
			}
			Console.WriteLine(MainWindow.Instance.EventConfiguration.Participants);
			MainWindow.Instance.Content = new HomeView();
		}
	}
}