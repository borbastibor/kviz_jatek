using kviz_jatek.Data;
using kviz_jatek.Model;
using System.Windows;

namespace kviz_jatek
{
    /// <summary>
    /// CreateRecordDialogBox.xaml
    /// A QuizContents tábla kiválasztott rekordjának a szerkesztése
    /// </summary>
    public partial class EditRecordDialogBox : Window
    {
        private readonly DatabaseContext context;   // referencia az adatbázishoz
        private readonly QuizContent editeditem;    // a szerkesztendő rekord

        // Konstruktor
        public EditRecordDialogBox(DatabaseContext dbcontext, QuizContent selecteditem)
        {
            InitializeComponent();
            context = dbcontext;
            editeditem = selecteditem;
            questionTextBox.Text = selecteditem.Question;           // a kiválasztott rekord adatainak a kiírása
            goodAnswerTextBox.Text = selecteditem.GoodAnswer;       // az adott TextBox-ba
            wrongAnswer1TextBox.Text = selecteditem.WrongAnswer1;
            wrongAnswer2TextBox.Text = selecteditem.WrongAnswer2;
        }

        // Beírt adatok validációja, meglévő rekord frissítése
        private void OnClick_Save(object sender, RoutedEventArgs e)
        {
            // Hibaüzenethez a MessageBox részleges előkészítése
            string caption = "Hiba";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;

            // A validáció csak annyira terjed ki, hogy megnézzük nem üresek-e a TextBox-ok
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
            // Ha idáig eljut, akkor elvileg frissíthejük a kiválasztott rekordot az adatbázisban
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
                // Ha nem sikerül a változásokat visszamenteni az adatbázisba, akkor hibaüzenet.
                string messageBoxText = "Nem sikerült menteni a változásokat!";
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
            
        }
    }
}