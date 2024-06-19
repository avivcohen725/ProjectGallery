using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using PersonManager.Models;

namespace PersonManager;

public partial class MainWindow : Window
{
    private const string FilePath = "people.json";
    private readonly ICollectionView _peopleView;

    public MainWindow()
    {
        InitializeComponent();

        People = new ObservableCollection<Person>();
        _peopleView = CollectionViewSource.GetDefaultView(People);

        PeopleDataGrid.ItemsSource = _peopleView;

        LoadData();
    }

    public ObservableCollection<Person> People { get; set; }

    private void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (PeopleDataGrid.SelectedItem is Person selectedPerson)
        {
            TB_ID.Text = selectedPerson.ID.ToString();
            TB_Name.Text = selectedPerson.Name;
            TB_Age.Text = selectedPerson.Age.ToString();
        }
    }

    private void HandleAddClick(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(TB_Age.Text, out int age) && !string.IsNullOrWhiteSpace(TB_Name.Text))
        {
            var newPerson = new Person
            {
                ID = GenerateID(),
                Name = TB_Name.Text,
                Age = age
            };

            People.Add(newPerson);
            SaveData();
            ClearForm();
        }
    }

    private void HandleUpdateClick(object sender, RoutedEventArgs e)
    {
        if (PeopleDataGrid.SelectedItem is Person selectedPerson &&
            int.TryParse(TB_Age.Text, out int age) &&
            !string.IsNullOrWhiteSpace(TB_Name.Text))
        {

            selectedPerson.Name = TB_Name.Text;
            selectedPerson.Age = age;

            PeopleDataGrid.Items.Refresh();
            SaveData();
            ClearForm();
        }
    }

    private void HandleDeleteClick(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Are you sure?", "Delete", MessageBoxButton.YesNo);
        if (result == MessageBoxResult.Yes && PeopleDataGrid.SelectedItem is Person personToDelete)
        {
            People.Remove(personToDelete);
            SaveData();
            ClearForm();
        }
    }

    private void HandleFilterKeyUp(object sender, KeyEventArgs e)
    {
        string filterString = TB_Filter.Text.ToLower();
        _peopleView.Filter = o => o is Person person && person.Name.ToLower().Contains(filterString);
    }

    private void HandleFilterFocus(object sender, RoutedEventArgs e)
    {
        if (TB_Filter.Text == "Filter...")
        {
            TB_Filter.Clear();
        }
    }

    private void HandleFilterLostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TB_Filter.Text))
        {
            TB_Filter.Text = "Filter...";
        }
    }

    private void ClearForm()
    {
        TB_ID.Clear();
        TB_Name.Clear();
        TB_Age.Clear();
        PeopleDataGrid.SelectedItem = null;
    }

    private void SaveData()
    {
        try
        {
            string rawData = JsonSerializer.Serialize(People);
            File.WriteAllText(FilePath, rawData);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save data: {ex.Message}");
        }
    }

    private void LoadData()
    {
        if (!File.Exists(FilePath)) return;

        try
        {
            string rawData = File.ReadAllText(FilePath);
            var people = JsonSerializer.Deserialize<List<Person>>(rawData) ?? new List<Person>();

            foreach (var person in people)
            {
                People.Add(person);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load data: {ex.Message}");
        }
    }

    private int GenerateID()
    {
        return People.Any() ? People.Max(p => p.ID) + 1 : 1;
    }
}
