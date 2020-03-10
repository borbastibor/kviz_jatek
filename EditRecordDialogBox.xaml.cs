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
        private QuizContent editeditem;

        public EditRecordDialogBox(DatabaseContext dbcontext, QuizContent selecteditem)
        {
            InitializeComponent();
            this.context = dbcontext;
            this.editeditem = selecteditem;
        }

        // Beírt adatok validációja, meglévő rekord frissítése
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
            var recordtoupdate = this.context.QuizContents.Find(this.editeditem.Id);
            if (recordtoupdate != null)
            {
                recordtoupdate.Question = questionTextBox.Text;
                recordtoupdate.GoodAnswer = goodAnswerTextBox.Text;
                recordtoupdate.WrongAnswer1 = wrongAnswer1TextBox.Text;
                recordtoupdate.WrongAnswer2 = wrongAnswer2TextBox.Text;
                this.context.SaveChanges();
            } else
            {
                string messageBoxText = "Nem sikerült menteni a változásokat!";
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
            
        }
    }
}