using kviz_jatek.Data;
using kviz_jatek.Model;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using System.Linq;
using System.Threading;
using System.Windows;

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

            // TopScore tábla feltöltése a initial_questions.csv-ből nyert adatokkal
            if (context.TopScores.ToList().Count() == 0)
            {
                using (TextFieldParser parser = new TextFieldParser("initial_questions.csv"))
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

            // TopScore tábla feltöltése a initial_topscores.csv-ből nyert adatokkal
            if (context.QuizContents.ToList().Count() == 0)
            {
                using (TextFieldParser parser = new TextFieldParser("initial_topscores.csv"))
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
            topscoreswindow = new TopScoresWindow(this);
            dbmanagerwindow = new DbManagerWindow(this);
        }

        // A kvíz játék ablakának a megjelenítése
        private void OnClick_StartQuiz(object sender, RoutedEventArgs e)
        {

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
            Close();
        }

        // A bezárás gombra kattintáskor zárja be a többi létrehozott ablakot 
        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            topscoreswindow.Close();
            dbmanagerwindow.Close();
        }
    }
}
