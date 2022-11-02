using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MUNManager.Configuration;
using MUNManager.Utils;


/*
 * TODO: Fix this dumpster fire of a UI (searching, less stupid sorting). Currently proof of concept only!
 */
namespace MUNManager.Views.Setup {
	public partial class CountrySelection : UserControl {
		private static readonly ObservableCollection<string> AvailableCountries = new(Country.AllString());
		private static readonly ObservableCollection<string> SelectedCountries = new();
		private static ListBox _selectedCountriesListBox = null!;
		private static AutoCompleteBox _searchBox = null!;
		public CountrySelection()
		{
			InitializeComponent();
			_searchBox = this.FindControl<AutoCompleteBox>("Countries");
			_selectedCountriesListBox = this.FindControl<ListBox>("Selected");

			_searchBox.Items = AvailableCountries;
			_selectedCountriesListBox.Items = SelectedCountries;

		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void EventSetup1(object? sender, RoutedEventArgs e)
		{
			MainWindow.Instance.Content = new EventSetup1();
		}

		private void RemoveFromSelected(object? sender, RoutedEventArgs e)
		{
			if (_selectedCountriesListBox.SelectedItems.Count == 0) return;

			AvailableCountries.Add(_selectedCountriesListBox.SelectedItems[0]?.ToString()!);
			SelectedCountries.Remove(_selectedCountriesListBox.SelectedItems[0]?.ToString()!);
			
			var tempSorted1 = new List<string>(AvailableCountries);
			tempSorted1.Sort();

			for (var i = 0; i < tempSorted1.Count; i++)
			{
				AvailableCountries.Move(AvailableCountries.IndexOf(tempSorted1[i]), i);
			}
			var tempSorted2 = new List<string>(SelectedCountries);
			tempSorted2.Sort();

			for (var i = 0; i < tempSorted2.Count; i++)
			{
				SelectedCountries.Move(SelectedCountries.IndexOf(tempSorted2[i]), i);
			}
		}

		private void AddToSelected(object? sender, RoutedEventArgs e)
		{
			// TODO: Add error message
			if (_searchBox.SelectedItem == null || SelectedCountries.Contains(_searchBox.SelectedItem.ToString())) return;

			SelectedCountries.Add(_searchBox.SelectedItem.ToString()!);
			AvailableCountries.Remove(_searchBox.SelectedItem.ToString()!);
			
			var tempSorted1 = new List<string>(AvailableCountries);
			tempSorted1.Sort();

			for (var i = 0; i < tempSorted1.Count; i++)
			{
				AvailableCountries.Move(AvailableCountries.IndexOf(tempSorted1[i]), i);
			}
			var tempSorted2 = new List<string>(SelectedCountries);
			tempSorted2.Sort();

			for (var i = 0; i < tempSorted2.Count; i++)
			{
				SelectedCountries.Move(SelectedCountries.IndexOf(tempSorted2[i]), i);
			}
		}

		private void Finalize(object? sender, RoutedEventArgs e)
		{
			// TODO: Add error message later
			if (SelectedCountries.Count == 0) return;
			
			MainWindow.Instance.EventConfiguration.Participants = string.Join('-', SelectedCountries);
			new SaveDialog().Show();
		}
	}
}