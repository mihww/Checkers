using System.ComponentModel;

namespace Checkers
{
    public partial class Game : INotifyPropertyChanged
    {
        public Board Board { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public Game()
        {
            Board = new Board();
            Player1 = new Player(ColorType.Red);
            Player2 = new Player(ColorType.White);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}