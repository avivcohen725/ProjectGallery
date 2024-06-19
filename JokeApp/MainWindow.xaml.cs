using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace JokeApp
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            LoadCategories();
        }

        private async void LoadCategories()
        {
            try
            {
                string response = await _client.GetStringAsync("https://v2.jokeapi.dev/categories");
                var categoriesResponse = JsonSerializer.Deserialize<CategoriesResponse>(response);
                if (categoriesResponse != null && categoriesResponse.Categories != null)
                {
                    foreach (var category in categoriesResponse.Categories)
                    {
                        CB_Categories.Items.Add(category);
                    }
                    CB_Categories.SelectedIndex = 0; // Select the first category by default
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load categories: {ex.Message}");
            }
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            string selectedCategory = CB_Categories.SelectedItem?.ToString() ?? "Any";
            TB_Joke.Text = "Loading joke...";
            try
            {
                string joke = await GetJokeFromAPI(selectedCategory);

                var jokeObj = JsonSerializer.Deserialize<JokeDTO>(joke);

                if (jokeObj == null)
                {
                    TB_Joke.Text = "No joke to show";
                    return;
                }

                TB_Joke.Text = jokeObj.Type == "single"
                    ? $"{jokeObj.Joke}"
                    : $"{jokeObj.Setup}\n-----\n{jokeObj.Delivery}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to get joke: {ex.Message}");
            }
        }

        private async Task<string> GetJokeFromAPI(string category)
        {
            string url = $"https://v2.jokeapi.dev/joke/{category}";
            string response = await _client.GetStringAsync(url);
            return response;
        }
    }
}
