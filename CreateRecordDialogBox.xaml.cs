using kviz_jatek.Data;
using kviz_jatek.Model;
using System.Windows;

namespace kviz_jatek
{
    /// <summary>
    /// CreateRecordDialogBox.xaml
    /// Ez az ablak kezeli az új rekordok létrehozását a QuizContents táblában.
    /// </summary>
    public partial class CreateRecordDialogBox : Window
    {
        private readonly DatabaseContext context; // referencia az adatbázishoz

        // Konstruktor
        public CreateRecordDialogBox(DatabaseContext dbcontext)
        {
            InitializeComponent();
            context = dbcontext;
        }

        // Beírt adatok validációja, új rekord létrehozása
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
            // Ha idáig eljut, akkor elvileg minden rendben és hozzáadhatjuk az adatbázishoz
            QuizContent ujrekord = new QuizContent
            {
                Question = questionTextBox.Text,
                GoodAnswer = goodAnswerTextBox.Text,
                WrongAnswer1 = wrongAnswer1TextBox.Text,
                WrongAnswer2 = wrongAnswer2TextBox.Text
            };
            context.QuizContents.Add(ujrekord);
            context.SaveChanges();
            Close();
        }
    }
}
