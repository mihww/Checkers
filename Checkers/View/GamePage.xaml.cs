using System.Windows.Controls;
using System.Windows.Input;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        public GamePage(object dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }
        private void Handle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MenuCommands menuCommands)
            {
                menuCommands.OnMouseDown(sender, e);
            }
        }
    }
}