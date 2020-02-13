using kviz_jatek.Data;
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
        private readonly DatabaseContext context;

        public DbManagerWindow(Window mwindow, DatabaseContext datacontext)
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

        // Új rekord létrehozása
        private void OnClick_NewRecord(object sender, RoutedEventArgs e)
        {
            CreateRecordDialogBox dlg = new CreateRecordDialogBox();
            dlg.Owner = this;
            dlg.ShowDialog();
        }

        // Rekord szerkesztése
        private void OnClick_EditRecord(object sender, RoutedEventArgs e)
        {

        }

        // Rekord törlése
        private void OnClick_DeleteRecord(object sender, RoutedEventArgs e)
        {

        }
    }
}
