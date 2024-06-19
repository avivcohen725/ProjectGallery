using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PokemonApiApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
        }

        private async void LoadData_Click(object sender, RoutedEventArgs e)
        {
            LoadingPanel.Visibility = Visibility.Visible;
            PokemonDataGrid.Visibility = Visibility.Collapsed;
            DetailsPanel.Visibility = Visibility.Collapsed;

            var pokemonList = await GetFirst100PokemonDetailsAsync();

            LoadingPanel.Visibility = Visibility.Collapsed;
            PokemonDataGrid.Visibility = Visibility.Visible;

            PokemonDataGrid.ItemsSource = pokemonList;
        }

        private async Task<List<Pokemon>> GetFirst100PokemonDetailsAsync()
        {
            var pokemonList = new List<Pokemon>();
            using (var client = new HttpClient())
            {
                string apiUrl = "https://pokeapi.co/api/v2/pokemon?limit=100";
                var response = await client.GetStringAsync(apiUrl);
                var pokemonResponse = JsonConvert.DeserializeObject<PokemonResponse>(response);
                foreach (var result in pokemonResponse.Results)
                {
                    var pokemonDetailResponse = await client.GetStringAsync(result.Url);
                    var pokemonData = JsonConvert.DeserializeObject<PokemonDetail>(pokemonDetailResponse);
                    pokemonList.Add(new Pokemon
                    {
                        Name = pokemonData.Name,
                        ImageUrl = pokemonData.Sprites.FrontDefault,
                        Specs = new List<Spec>
                        {
                            new Spec { Name = "Height", Value = pokemonData.Height.ToString() },
                            new Spec { Name = "Weight", Value = pokemonData.Weight.ToString() }
                            // Add more specs if needed
                        }
                    });
                }
            }
            return pokemonList;
        }

        private void PokemonDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PokemonDataGrid.SelectedItem is Pokemon selectedPokemon)
            {
                PokemonImage.Source = new BitmapImage(new Uri(selectedPokemon.ImageUrl));
                PokemonName.Text = selectedPokemon.Name;
                SpecsPanel.Children.Clear();
                foreach (var spec in selectedPokemon.Specs)
                {
                    var specTextBlock = new TextBlock
                    {
                        Text = $"{spec.Name}: {spec.Value}",
                        Margin = new Thickness(0, 2, 0, 2)
                    };
                    SpecsPanel.Children.Add(specTextBlock);
                }
                DetailsPanel.Visibility = Visibility.Visible;
            }
        }
    }

    public class Pokemon
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public List<Spec> Specs { get; set; }
    }

    public class Spec
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class PokemonResponse
    {
        public List<Result> Results { get; set; }
    }

    public class Result
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class PokemonDetail
    {
        public string Name { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public Sprites Sprites { get; set; }
    }

    public class Sprites
    {
        [JsonProperty("front_default")]
        public string FrontDefault { get; set; }
    }
}
