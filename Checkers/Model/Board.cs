using System.Collections.ObjectModel;
using System.ComponentModel;


namespace Checkers
{
    public class Board : INotifyPropertyChanged
    {
        public ObservableCollection<ObservableCollection<Piece>> Pieces { get; set; }

        public Board()
        {
            Pieces = new ObservableCollection<ObservableCollection<Piece>>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}