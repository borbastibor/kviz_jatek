using kviz_jatek.Data;
using System.Threading;
using System.Windows;

namespace kviz_jatek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Window topscoreswindow;
        private readonly Window dbmanagerwindow;
        //private readonly DatabaseContext context;

        public MainWindow()
        {
            InitializeComponent();

            topscoreswindow = new TopScoresWindow(this);
            dbmanagerwindow = new DbManagerWindow(this);
        }

        // A kvíz játék ablakának a megjelenítése
        private void OnClick_StartQuiz(object sender, RoutedEventArgs e)
        {

        }

        // Az adatbáziskezelő ablak megjelenítése
        private void OnClick_Dbmanager(object sender, RoutedEventArgs e)
        {
            Hide();
            Thread.Sleep(150);
            dbmanagerwindow.Show();
        }

        // A legjobb eredmények ablak megjelenítése
        private void OnClick_TopScores(object sender, RoutedEventArgs e)
        {
            Hide();
            Thread.Sleep(150);
            topscoreswindow.Show();
        }

        // Kilépés gombra kattintás
        private void OnClick_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // A bezárás gombra kattintáskor zárja be a többi létrehozott ablakot 
        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            topscoreswindow.Close();
            dbmanagerwindow.Close();
        }
    }
}
