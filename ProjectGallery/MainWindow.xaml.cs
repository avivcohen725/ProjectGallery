using System.Windows;
using System.Windows.Controls;
using Common;
using ProjectGallery.Controls;

namespace ProjectGallery
{
    public partial class MainWindow : Window
    {
        private IProjectMeta[] projects = new IProjectMeta[]
        {
            new JokeApp.Project(),
            new MemoryGame.Project(),
            new PersonManager.Project(),
            new PokemonApiApp.Project(),
            new Pong.Project(),
            new TicTacToe.Project()
        };

        public MainWindow()
        {
            InitializeComponent();
            InitializeProjectButtons();
        }

        private void InitializeProjectButtons()
        {
            foreach (var project in projects)
            {
                ProjectButton button = new ProjectButton(project)
                {
                    Margin = new Thickness(10),
                    Width = 100,
                    Height = 130
                };
                button.Click += ProjectButton_Click;
                ProjectsWrapPanel.Children.Add(button);
            }
        }

        private void ProjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ProjectButton button)
            {
                var project = button.Project;
                if (project is PokemonApiApp.Project)
                {
                    var window = new PokemonApiApp.MainWindow();
                    window.Show();
                }

            }
        }
    }
}
