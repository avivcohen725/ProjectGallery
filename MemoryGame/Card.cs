namespace MemoryGame
{
    public class Card
    {
        public string ImagePath { get; set; }
        public bool IsMatched { get; set; }
        public bool IsFlipped { get; set; }

        public Card(string imagePath)
        {
            ImagePath = imagePath;
            IsMatched = false;
            IsFlipped = false;
        }
    }
}
