﻿using kviz_jatek.Data;
using kviz_jatek.Model;
using System.Windows;

namespace kviz_jatek
{
    /// <summary>
    /// Interaction logic for CreateRecordDialogBox.xaml
    /// </summary>
    public partial class CreateRecordDialogBox : Window
    {
        private readonly DatabaseContext context;

        public CreateRecordDialogBox(DatabaseContext dbcontext)
        {
            InitializeComponent();
            this.context = dbcontext;
        }

        // Beírt adatok validációja, új rekord létrehozása
        private void OnClick_Save(object sender, RoutedEventArgs e)
        {
            string caption = "Hiba";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;
            
            if (this.questionTextBox.Text == "")
            {
                string messageBoxText = "Nem adott meg kérdést!";
                MessageBox.Show(messageBoxText, caption, button, icon);
                return;
            }
            if (this.goodAnswerTextBox.Text == "")
            {
                string messageBoxText = "Nem adott meg helyes választ!";
                MessageBox.Show(messageBoxText, caption, button, icon);
                return;
            }
            if (this.wrongAnswer1TextBox.Text == "")
            {
                string messageBoxText = "Nem adott meg egy rossz választ!";
                MessageBox.Show(messageBoxText, caption, button, icon);
                return;
            }
            if (this.wrongAnswer2TextBox.Text == "")
            {
                string messageBoxText = "Nem adott meg egy rossz választ!";
                MessageBox.Show(messageBoxText, caption, button, icon);
                return;
            }
            QuizContent ujrekord = new QuizContent
            {
                Question = this.questionTextBox.Text,
                GoodAnswer = this.goodAnswerTextBox.Text,
                WrongAnswer1 = this.wrongAnswer1TextBox.Text,
                WrongAnswer2 = this.wrongAnswer2TextBox.Text
            };
            this.context.QuizContents.Add(ujrekord);
            this.context.SaveChanges();
        }
    }
}
