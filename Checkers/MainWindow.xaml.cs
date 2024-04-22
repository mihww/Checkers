using Checkers;
using System.Windows;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MenuCommands();
            MainFrame.Navigate(new GamePage(DataContext));
        }
    }
}