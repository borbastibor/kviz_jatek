using kviz_jatek.Data;
using kviz_jatek.Model;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace kviz_jatek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ISampleService sampleService;
        private readonly AppSettings settings;
        private readonly Window topscoreswindow;
        private readonly Window dbmanagerwindow;
        private readonly Window quizwindow;
        private readonly DatabaseContext context;

        public MainWindow(ISampleService sampleService, IOptions<AppSettings> settings, DatabaseContext dataContext)
        {
            InitializeComponent();
            this.sampleService = sampleService;
            this.settings = settings.Value;
            context = dataContext;

            // Tesztadatokkal történő feltöltés
            // A QuizContents tábla feltöltése a initial_questions.csv-ből nyert kezdő adatokkal, ha táblában 0 rekord van
            if (context.QuizContents.ToList().Count() == 0)
            {
                using (TextFieldParser parser = new TextFieldParser("initial_questions.csv", Encoding.UTF8))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(";");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        QuizContent temp_quizcontent = new QuizContent
                        {
                            Question = fields[0],
                            GoodAnswer = fields[1],
                            WrongAnswer1 = fields[2],
                            WrongAnswer2 = fields[3]
                        };
                        context.QuizContents.Add(temp_quizcontent);
                    }
                    context.SaveChanges();
                }
            }

            // Tesztadatokkal történő feltöltés
            // A TopScores tábla feltöltése a initial_topscores.csv-ből nyert kezdő adatokkal, ha a táblában 0 rekord van
            if (context.TopScores.ToList().Count() == 0)
            {
                using (TextFieldParser parser = new TextFieldParser("initial_topscores.csv", Encoding.UTF8))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(";");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        TopScore temp_topscore = new TopScore
                        {
                            Name = fields[0],
                            Score = int.Parse(fields[1])
                        };
                        context.TopScores.Add(temp_topscore);
                    }
                    context.SaveChanges();
                }
            }

            // A többi ablak létrehozása
            topscoreswindow = new TopScoresWindow(this, context);
            dbmanagerwindow = new DbManagerWindow(this, context);
            quizwindow = new QuizWindow(this, context);
        }

        // A kvíz játék ablakának a megjelenítése
        private void OnClick_StartQuiz(object sender, RoutedEventArgs e)
        {
            Hide();
            Thread.Sleep(150);
            quizwindow.Show();
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
            string messageBoxText = "Biztos, hogy kilép?";
            string caption = "Kilépés a programból";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    CloseAllWindow();
                    Close();
                    break;
                case MessageBoxResult.No:
                    break;
            }
            
        }

        // Ablak mozgatása lenyomott egérgombbal
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        // Öszes ablak bezárása
        private void CloseAllWindow()
        {
            topscoreswindow.Close();
            dbmanagerwindow.Close();
            quizwindow.Close();
        }

    }
}
