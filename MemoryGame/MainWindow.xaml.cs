using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MemoryGame
{
    public partial class MainWindow : Window
    {
        private List<Card> _cards;
        private Button _firstClicked, _secondClicked;
        private readonly Random _random = new Random();
        private int _player1Score = 0;
        private int _player2Score = 0;
        private bool _isPlayer1Turn = true;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
            UpdateTurnIndicator();
        }

        private void InitializeGame()
        {
            _cards = new List<Card>();
            var cardImages = new List<string>
            {
                "Resources/image1.png", "Resources/image2.png", "Resources/image3.png", "Resources/image4.png",
                "Resources/image5.png", "Resources/image6.png", "Resources/image7.png", "Resources/image8.png",
                "Resources/image1.png", "Resources/image2.png", "Resources/image3.png", "Resources/image4.png",
                "Resources/image5.png", "Resources/image6.png", "Resources/image7.png", "Resources/image8.png"
            };

            foreach (var image in cardImages.OrderBy(x => _random.Next()))
            {
                _cards.Add(new Card(image));
            }

            foreach (var card in _cards)
            {
                var button = new Button
                {
                    Content = new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/MemoryGame;component/Resources/question_mark.png")),
                        Stretch = System.Windows.Media.Stretch.Uniform
                    },
                    Tag = card,
                    FontSize = 24
                };
                button.Click += Card_Click;
                CardGrid.Children.Add(button);
            }
        }

        private void Card_Click(object sender, RoutedEventArgs e)
        {
            if (_firstClicked != null && _secondClicked != null)
                return;

            var clickedButton = sender as Button;
            var clickedCard = clickedButton.Tag as Card;

            if (clickedCard.IsFlipped || clickedCard.IsMatched)
                return;

            clickedButton.Content = new Image
            {
                Source = new BitmapImage(new Uri($"pack://application:,,,/MemoryGame;component/{clickedCard.ImagePath}")),
                Stretch = System.Windows.Media.Stretch.Uniform
            };
            clickedCard.IsFlipped = true;

            if (_firstClicked == null)
            {
                _firstClicked = clickedButton;
                return;
            }

            _secondClicked = clickedButton;

            if ((clickedButton.Tag as Card).ImagePath == (_firstClicked.Tag as Card).ImagePath)
            {
                (_firstClicked.Tag as Card).IsMatched = true;
                (clickedButton.Tag as Card).IsMatched = true;
                UpdateScore();
                ResetSelections();
            }
            else
            {
                var timer = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                timer.Tick += Timer_Tick;
                timer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var timer = sender as System.Windows.Threading.DispatcherTimer;
            timer.Stop();

            _firstClicked.Content = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/MemoryGame;component/Resources/question_mark.png")),
                Stretch = System.Windows.Media.Stretch.Uniform
            };
            _secondClicked.Content = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/MemoryGame;component/Resources/question_mark.png")),
                Stretch = System.Windows.Media.Stretch.Uniform
            };
            (_firstClicked.Tag as Card).IsFlipped = false;
            (_secondClicked.Tag as Card).IsFlipped = false;
            ResetSelections();
            SwitchPlayer();
            UpdateTurnIndicator();
        }

        private void UpdateScore()
        {
            if (_isPlayer1Turn)
            {
                _player1Score++;
                Player1Score.Text = _player1Score.ToString();
            }
            else
            {
                _player2Score++;
                Player2Score.Text = _player2Score.ToString();
            }
        }

        private void ResetSelections()
        {
            _firstClicked = null;
            _secondClicked = null;
        }

        private void SwitchPlayer()
        {
            _isPlayer1Turn = !_isPlayer1Turn;
        }

        private void UpdateTurnIndicator()
        {
            if (_isPlayer1Turn)
            {
                PlayerTurnIndicator.Text = "Player 1's Turn";
            }
            else
            {
                PlayerTurnIndicator.Text = "Player 2's Turn";
            }
        }
    }
}
