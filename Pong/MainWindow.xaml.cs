using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Pong
{
    public partial class MainWindow : Window
    {
        private double paddleSpeed = 15;
        private Rectangle paddle1;
        private Rectangle paddle2;
        private Rectangle ball;
        private DispatcherTimer gameTimer;

        private double ballSpeedY = 5;
        private double ballSpeedX = 5;
        private int player1Score = 0;
        private int player2Score = 0;
        private TextBlock scoreTextPlayer1;
        private TextBlock scoreTextPlayer2;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGameElements();
            InitializeEventHandlers();
            InitializeGameTimer();
        }

        private void InitializeGameElements()
        {
            GameCanvas.Background = Brushes.Black;

            paddle1 = CreatePlayerRectangle();
            paddle2 = CreatePlayerRectangle();
            ball = new Rectangle
            {
                Width = 15,
                Height = 15,
                Fill = Brushes.White
            };

            scoreTextPlayer1 = new TextBlock
            {
                FontSize = 24,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };

            scoreTextPlayer2 = new TextBlock
            {
                FontSize = 24,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };

            // Draw divider and scores
            DrawDividerAndScores();

            GameCanvas.Children.Add(paddle1);
            GameCanvas.Children.Add(paddle2);
            GameCanvas.Children.Add(ball);
            GameCanvas.Children.Add(scoreTextPlayer1);
            GameCanvas.Children.Add(scoreTextPlayer2);

            UpdateScore();
        }

        private void DrawDividerAndScores()
        {
            // Divider line
            var divider = new Line
            {
                X1 = GameCanvas.ActualWidth / 2,
                Y1 = 0,
                X2 = GameCanvas.ActualWidth / 2,
                Y2 = GameCanvas.ActualHeight,
                Stroke = Brushes.White,
                StrokeThickness = 2
            };
            GameCanvas.Children.Add(divider);

            // Score text positions
            Canvas.SetLeft(scoreTextPlayer1, GameCanvas.ActualWidth / 4 - scoreTextPlayer1.ActualWidth / 2);
            Canvas.SetTop(scoreTextPlayer1, 20);

            Canvas.SetLeft(scoreTextPlayer2, 3 * GameCanvas.ActualWidth / 4 - scoreTextPlayer2.ActualWidth / 2);
            Canvas.SetTop(scoreTextPlayer2, 20);
        }

        private void InitializeEventHandlers()
        {
            Loaded += WindowLoaded;
            SizeChanged += HandleWindowSizeChanged;
            KeyDown += HandleKeyDown;
        }

        private void InitializeGameTimer()
        {
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(16);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
        }

        private void GameLoop(object? sender, EventArgs e)
        {
            MoveBall();
            CheckCollisions();
            MovePaddle2();
        }

        private void MoveBall()
        {
            Canvas.SetLeft(ball, Canvas.GetLeft(ball) + ballSpeedX);
            Canvas.SetTop(ball, Canvas.GetTop(ball) + ballSpeedY);
        }

        private void CheckCollisions()
        {
            // Ball collision with top and bottom walls
            if (Canvas.GetTop(ball) <= 0 || Canvas.GetTop(ball) >= GameCanvas.ActualHeight - ball.Height)
            {
                ballSpeedY = -ballSpeedY;
            }

            // Ball collision with paddles
            if (BallCollidesWithPaddle(paddle1) || BallCollidesWithPaddle(paddle2))
            {
                ballSpeedX = -ballSpeedX;
            }

            // Ball out of bounds (left or right side)
            if (Canvas.GetLeft(ball) <= 0)
            {
                player2Score++;
                UpdateScore();
                ResetBall();
            }
            else if (Canvas.GetLeft(ball) >= GameCanvas.ActualWidth - ball.Width)
            {
                player1Score++;
                UpdateScore();
                ResetBall();
            }
        }

        private bool BallCollidesWithPaddle(Rectangle paddle)
        {
            return Canvas.GetLeft(ball) <= Canvas.GetLeft(paddle) + paddle.Width &&
                   Canvas.GetLeft(ball) + ball.Width >= Canvas.GetLeft(paddle) &&
                   Canvas.GetTop(ball) + ball.Height >= Canvas.GetTop(paddle) &&
                   Canvas.GetTop(ball) <= Canvas.GetTop(paddle) + paddle.Height;
        }

        private void MovePaddle2()
        {
            double p2Top = Canvas.GetTop(paddle2);
            double ballTop = Canvas.GetTop(ball);

            if (p2Top + paddle2.Height / 2 < ballTop + ball.Height / 2 && p2Top < GameCanvas.ActualHeight - paddle2.Height)
            {
                Canvas.SetTop(paddle2, p2Top + paddleSpeed / 2);
            }
            else if (p2Top + paddle2.Height / 2 > ballTop + ball.Height / 2 && p2Top > 0)
            {
                Canvas.SetTop(paddle2, p2Top - paddleSpeed / 2);
            }
        }

        private void ResetBall()
        {
            Canvas.SetLeft(ball, (GameCanvas.ActualWidth - ball.Width) / 2);
            Canvas.SetTop(ball, (GameCanvas.ActualHeight - ball.Height) / 2);
            RandomizeBallSpeed();
        }

        private void RandomizeBallSpeed()
        {
            var random = new Random();
            ballSpeedX = random.Next(4, 6) * (random.Next(2) == 0 ? -1 : 1);
            ballSpeedY = random.Next(4, 6) * (random.Next(2) == 0 ? -1 : 1);
        }

        private void UpdateScore()
        {
            scoreTextPlayer1.Text = $"{player1Score}";
            scoreTextPlayer2.Text = $"{player2Score}";
        }

        private void HandleWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetInitialPositions();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            SetInitialPositions();
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            double p1Top = Canvas.GetTop(paddle1);

            if (e.Key == Key.W && p1Top > 0)
            {
                Canvas.SetTop(paddle1, p1Top - paddleSpeed);
            }
            if (e.Key == Key.S && p1Top < (GameCanvas.ActualHeight - paddle1.Height))
            {
                Canvas.SetTop(paddle1, p1Top + paddleSpeed);
            }
        }

        private void SetInitialPositions()
        {
            Canvas.SetLeft(paddle1, 50);
            Canvas.SetTop(paddle1, (GameCanvas.ActualHeight - paddle1.Height) / 2);

            Canvas.SetLeft(paddle2, GameCanvas.ActualWidth - 60);
            Canvas.SetTop(paddle2, (GameCanvas.ActualHeight - paddle2.Height) / 2);

            Canvas.SetLeft(ball, (GameCanvas.ActualWidth - ball.Width) / 2);
            Canvas.SetTop(ball, (GameCanvas.ActualHeight - ball.Height) / 2);

            // Update divider and scores positions on window size change
            DrawDividerAndScores();
        }

        private Rectangle CreatePlayerRectangle()
        {
            return new Rectangle
            {
                Width = 10,
                Height = 100,
                Fill = Brushes.White
            };
        }
    }
}
