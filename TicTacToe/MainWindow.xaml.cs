﻿using System.ComponentModel;
using System.Windows;
using TicTacToe.Enums;
using TicTacToe.EventArgs;

namespace TicTacToe
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int playerOneScore = 0;
        private int playerTwoScore = 0;
        private string endGameState = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            MyBoard.GameEnded += HandleGameEnded;
            DataContext = this;
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void HandleGameEnded(object? sender, GameEndEventArgs e)
        {
            switch (e.GameResult)
            {
                case GameResult.PlayerOneWins:
                    PlayerOneScore++;
                    EndGameState = "Player 1 won!";
                    break;
                case GameResult.PlayerTwoWins:
                    PlayerTwoScore++;
                    EndGameState = "Player 2 won!";
                    break;
                case GameResult.Draw:
                    EndGameState = "Draw!";
                    break;
            }
        }

        public string EndGameState
        {
            get => endGameState;
            set
            {
                endGameState = value;
                OnPropertyChanged(nameof(EndGameState));
            }
        }

        public int PlayerOneScore
        {
            get => playerOneScore;
            set
            {
                playerOneScore = value;
                OnPropertyChanged(nameof(PlayerOneScore));
            }
        }

        public int PlayerTwoScore
        {
            get => playerTwoScore;
            set
            {
                playerTwoScore = value;
                OnPropertyChanged(nameof(PlayerTwoScore));
            }
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            GameType gameType;

            if (sender == Btn_PvP)
            {
                gameType = GameType.PvP;
            }
            else if (sender == Btn_PvC)
            {
                gameType = GameType.PvC;
            }
            else if (sender == Btn_CvC)
            {
                gameType = GameType.CvC;
            }
            else
            {
                return;
            }

            MyBoard.StartNewGame(gameType);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
