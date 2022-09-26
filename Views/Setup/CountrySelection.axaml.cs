using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		private static ObservableCollection<string> _availableCountries = null!;
		private static ObservableCollection<string> _selectedCountries = null!;
		private static ListBox _availableCountriesListBox = null!;
		private static ListBox _selectedCountriesListBox = null!;
		public CountrySelection()
		{
			InitializeComponent();
			_selectedCountries = new ObservableCollection<string>();
			_availableCountries = new ObservableCollection<string>(Country.AllString());
			
			_availableCountriesListBox = this.FindControl<ListBox>("Available");
			_selectedCountriesListBox = this.FindControl<ListBox>("Selected");

			_availableCountriesListBox.Items = _availableCountries;
			_selectedCountriesListBox.Items = _selectedCountries;

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

			_availableCountries.Add(_selectedCountriesListBox.SelectedItems[0]?.ToString()!);
			_selectedCountries.Remove(_selectedCountriesListBox.SelectedItems[0]?.ToString()!);
			
			var tempSorted1 = new List<string>(_availableCountries);
			tempSorted1.Sort();

			for (var i = 0; i < tempSorted1.Count; i++)
			{
				_availableCountries.Move(_availableCountries.IndexOf(tempSorted1[i]), i);
			}
			var tempSorted2 = new List<string>(_selectedCountries);
			tempSorted2.Sort();

			for (var i = 0; i < tempSorted2.Count; i++)
			{
				_selectedCountries.Move(_selectedCountries.IndexOf(tempSorted2[i]), i);
			}
		}

		private void AddToSelected(object? sender, RoutedEventArgs e)
		{
			if (_availableCountriesListBox.SelectedItems.Count == 0) return;

			_selectedCountries.Add(_availableCountriesListBox.SelectedItems[0].ToString()!);
			_availableCountries.Remove(_availableCountriesListBox.SelectedItems[0].ToString()!);
			
			var tempSorted1 = new List<string>(_availableCountries);
			tempSorted1.Sort();

			for (var i = 0; i < tempSorted1.Count; i++)
			{
				_availableCountries.Move(_availableCountries.IndexOf(tempSorted1[i]), i);
			}
			var tempSorted2 = new List<string>(_selectedCountries);
			tempSorted2.Sort();

			for (var i = 0; i < tempSorted2.Count; i++)
			{
				_selectedCountries.Move(_selectedCountries.IndexOf(tempSorted2[i]), i);
			}
		}

		private void Finalize(object? sender, RoutedEventArgs e)
		{
			// Add error message later
			if (_selectedCountries.Count == 0/*&& !VolatileConstants.Debug*/) return;
			VolatileConfiguration.Participants = _selectedCountries.ToList();
			new SaveDialog().Show();
		}
	}
}