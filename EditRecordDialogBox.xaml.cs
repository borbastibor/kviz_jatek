using kviz_jatek.Data;
using kviz_jatek.Model;
using System.Windows;

namespace kviz_jatek
{
    /// <summary>
    /// Interaction logic for CreateRecordDialogBox.xaml
    /// </summary>
    public partial class EditRecordDialogBox : Window
    {
        private readonly DatabaseContext context;
        private readonly QuizContent editeditem;

        public EditRecordDialogBox(DatabaseContext dbcontext, QuizContent selecteditem)
        {
            InitializeComponent();
            context = dbcontext;
            editeditem = selecteditem;
            questionTextBox.Text = selecteditem.Question;
            goodAnswerTextBox.Text = selecteditem.GoodAnswer;
            wrongAnswer1TextBox.Text = selecteditem.WrongAnswer1;
            wrongAnswer2TextBox.Text = selecteditem.WrongAnswer2;
        }

        // Beírt adatok validációja, meglévő rekord frissítése
        private void OnClick_Save(object sender, RoutedEventArgs e)
        {
            string caption = "Hiba";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;

            if (questionTextBox.Text == "")
            {
                string messageBoxText = "Nem adott meg kérdést!";
                MessageBox.Show(messageBoxText, caption, button, icon);
                return;
            }
            if (goodAnswerTextBox.Text == "")
            {
                string messageBoxText = "Nem adott meg helyes választ!";
                MessageBox.Show(messageBoxText, caption, button, icon);
                return;
            }
            if (wrongAnswer1TextBox.Text == "")
            {
                string messageBoxText = "Nem adott meg egy rossz választ!";
                MessageBox.Show(messageBoxText, caption, button, icon);
                return;
            }
            if (wrongAnswer2TextBox.Text == "")
            {
                string messageBoxText = "Nem adott meg egy rossz választ!";
                MessageBox.Show(messageBoxText, caption, button, icon);
                return;
            }
            QuizContent recordtoupdate = context.QuizContents.Find(editeditem.Id);
            if (recordtoupdate != null)
            {
                recordtoupdate.Question = questionTextBox.Text;
                recordtoupdate.GoodAnswer = goodAnswerTextBox.Text;
                recordtoupdate.WrongAnswer1 = wrongAnswer1TextBox.Text;
                recordtoupdate.WrongAnswer2 = wrongAnswer2TextBox.Text;
                context.SaveChanges();
                Close();
            } else
            {
                string messageBoxText = "Nem sikerült menteni a változásokat!";
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
            
        }
    }
}