﻿using kviz_jatek.Data;
using kviz_jatek.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace kviz_jatek
{
    /// <summary>
    /// Ebben az ablakban lehet rekordokat létrehozni, szerkeszteni és törölni a kérdések adatbázisából.
    /// </summary>
    public partial class DbManagerWindow : Window
    {
        private readonly Window mainwindow;         // referencia a főmenühöz
        private readonly DatabaseContext context;   // referencia az adatbázishoz

        public DbManagerWindow(Window mwindow, DatabaseContext datacontext)
        {
            InitializeComponent();
            mainwindow = mwindow;
            context = datacontext;
        }

        // Vissza a főmenű ablakába
        private void OnClick_BackToMenu(object sender, RoutedEventArgs e)
        {
            Hide();                 // Elöszőr elrejti a Quizwindow-t
            Thread.Sleep(150);      // Vár 150ms-t
            mainwindow.Show();      // Visszatér a főmenü ablakához
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
                string messageBoxText = "Biztos, hogy törli a kijelölt rekordot?";
                string caption = "Rekord törlése";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Question;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        context.QuizContents.Remove((QuizContent)QuestionListView.SelectedItem);
                        context.SaveChanges();
                        ReloadListViewContent();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        // Az ablak aktivizálásra megjeleníti a kérdések listáját
        private void OnActivated(object sender, System.EventArgs e)
        {
            ReloadListViewContent();
        }

        // Belső függvény, ami újratölti az listanézet tartalmát
        private void ReloadListViewContent()
        {
            List<QuizContent> questionlist = context.QuizContents.ToList();
            QuestionListView.ItemsSource = questionlist;
        }
    }
}
