using TicTacToe.Enums;

namespace TicTacToe.EventArgs
{
    public class GameEndEventArgs : System.EventArgs
    {
        public GameEndEventArgs(GameResult result)
        {
            GameResult = result;
        }

        public GameResult GameResult { get; set; }
    }
}
