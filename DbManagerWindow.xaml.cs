using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace kviz_jatek
{
    /// <summary>
    /// Interaction logic for DbManagerWindow.xaml
    /// </summary>
    public partial class DbManagerWindow : Window
    {
        private readonly Window mainwindow;

        public DbManagerWindow(Window mwindow)
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
