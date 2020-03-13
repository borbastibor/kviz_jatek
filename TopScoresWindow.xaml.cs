using kviz_jatek.Data;
using kviz_jatek.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace kviz_jatek
{
    /// <summary>
    /// TopScoresWindow.xaml
    /// A legjobb eredméynek ablak kezelése történik itt.
    /// Betöltődéskor kiírja a TopScores tábla összes elemét.
    /// A legjobb eredmények törölhetők.
    /// </summary>
    public partial class TopScoresWindow : Window
    {
        private Window mainwindow;                  // referencia a főmenü ablakához
        private readonly DatabaseContext context;   // referencia az adatbázishoz

        // Konstruktor
        public TopScoresWindow(Window mwindow, DatabaseContext datacontext)
        {
            InitializeComponent();
            mainwindow = mwindow;
            context = datacontext;
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

        // Legjobb eredmények törlése
        private void OnClick_DeleteTopScores(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Biztosan törli az eredményeket?";
            string caption = "Legjobb eredmények törlése";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    context.Database.ExecuteSqlRaw("DELETE FROM TopScores");
                    break;
                case MessageBoxResult.No:
                    break;
            }
            List<TopScore> topscorelist = context.TopScores.ToList();
            TopScoreListView.ItemsSource = topscorelist;
        }

        // Amint aktiválódik az ablak, betöltjük az adatokat a TopScores táblából
        private void OnActivated(object sender, System.EventArgs e)
        {
            List<TopScore> topscorelist = context.TopScores.ToList();
            TopScoreListView.ItemsSource = topscorelist;
        }
    }
}
