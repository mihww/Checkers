using System;
using System.ComponentModel;

namespace Checkers
{
    public class Piece : INotifyPropertyChanged
    {
        public PieceType _type;
        public PieceType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                NotifyPropertyChanged("Type");
            }
        }


        public ColorType _color;
        public ColorType Color
        {
            get { return _color; }
            set
            {
                _color = value;
                NotifyPropertyChanged("Color");
            }
        }

        private Tuple<int, int> _coordonates;
        public Tuple<int, int> Coordonates
        {
            get { return _coordonates; }
            set
            {
                _coordonates = value;
                NotifyPropertyChanged("Coordonates");
            }
        }

        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                NotifyPropertyChanged("ImagePath");
            }
        }

        public Piece()
        {
            Color = ColorType.None;
            Type = PieceType.None;
            ImagePath = "";
            Coordonates = Tuple.Create(-1, -1);
        }

        public Piece(PieceType type, ColorType color, String imagePath, Tuple<int, int> coordonates)
        {
            Type = type;
            Color = color;
            ImagePath = imagePath;
            Coordonates = coordonates;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}