using kviz_jatek.Data;
using kviz_jatek.Model;
using System.Collections.Generic;
using System.Linq;
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

        // Új rekord létrehozása
        private void OnClick_NewRecord(object sender, RoutedEventArgs e)
        {
            CreateRecordDialogBox dlg = new CreateRecordDialogBox(context);
            dlg.Owner = this;
            dlg.ShowDialog();
            dlg.Close();
        }

        // Rekord szerkesztése
        private void OnClick_EditRecord(object sender, RoutedEventArgs e)
        {
            if (QuestionListView.SelectedItem != null)
            {
                EditRecordDialogBox dlg = new EditRecordDialogBox(context, (QuizContent)QuestionListView.SelectedItem);
                dlg.Owner = this;
                dlg.ShowDialog();
                dlg.Close();
            } 
        }

        // Rekord törlése
        private void OnClick_DeleteRecord(object sender, RoutedEventArgs e)
        {
            if (QuestionListView.SelectedItem != null)
            {

            }
        }

        // Az ablak aktivizálásra megjeleníti a kérdések listáját
        private void OnActivated(object sender, System.EventArgs e)
        {
            List<QuizContent> questionlist = context.QuizContents.ToList();
            QuestionListView.ItemsSource = questionlist;
        }
    }
}
