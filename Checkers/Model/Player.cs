using System.ComponentModel;

namespace Checkers
{
    public class Player : INotifyPropertyChanged
    {
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

        public Player(ColorType color)
        {
            Color = color;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}