using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace kviz_jatek
{
    /// <summary>
    /// Interaction logic for TopScoresWindow.xaml
    /// </summary>
    public partial class TopScoresWindow : Window
    {
        private Window mainwindow;

        public TopScoresWindow(Window mwindow)
        {
            InitializeComponent();
            mainwindow = mwindow;
        }

        // Vissza a főmenű ablakába
        private void OnClick_BackToMenu(object sender, RoutedEventArgs e)
        {
            Hide();
            Thread.Sleep(150);
            mainwindow.Show();
        }

        // Ablak mozgatása lenyomott egérgombbal
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
