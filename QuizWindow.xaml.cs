using kviz_jatek.Data;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace kviz_jatek
{
    /// <summary>
    /// QuizWindow.xaml
    /// Ebben az ablakban jelennek meg az adatbázisból véletlenszerűen kiválasztott kérdések és a hozzájuk tartozó (jó-rossz)válaszok
    /// A kitöltés után a válaszok kiértékelésre kerülnek és függően a pontszámtól felkerül a játékos a legjobb pontszámok táblázatába
    /// ide vaalmit beleírunk bala
    /// </summary>
    public partial class QuizWindow : Window
    {
        private readonly Window mainwindow;         // referencia a főmenühöz
        private readonly DatabaseContext context;   // referencia az adatbázis eléréséhez

        // Konstruktor
        public QuizWindow(Window mwindow, DatabaseContext datacontext)
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

        // Kérdéssor kiértékelése
        private void OnClick_GetResult(object sender, RoutedEventArgs e)
        {
            /// Itt kellene elvégezni a kérdéssor kiértékelését. Felhasználva a kiválasztott kérdések Id-ét be lehetne olvasni a RadioButton-k állását.
            /// Viszont a jó és rossz válaszok sorrendje nem egyezik meg minden kérdésnél, mivel összekevertük őket a kiíratásnál (OnActivated függvény),
            /// ezért kell valamilyen megoldás, amivel megkülönböztethetjük őket. Egy lehetséges megoldás, hogy az Id alapján összehasonlítjuk az adott
            /// RadioButton.Text property-ét a QuizContents táblából, az Id alapján, kikeresett rekord jó és rossz válaszaival.
            /// 
            /// Ha meg van a jó válaszok száma, akkor egy egyszerű dialog-ban ki lehet írni az eredményt, illetve ha mondjuk eléri a 90%-os teljesítményt,
            /// akkor egy dialog-on bekérjük a nevét és létrehozunk egy új rekordot a TopScores táblában a megadott névvel és pontszámmal.
        }

        // Kérdések véletlen kiválasztása és kiírása
        private void OnActivated(object sender, System.EventArgs e)
        {
            /// Itt kellene, a stackpanel-be ágyazottan (StackPanel.Children), dinamikusan (kódból) létrehozott wpf elemekkel (TextBlock, RadioButton) megjeleníteni a kérdéseket
            /// és a hozzájuk tartozó válaszokat. Az új wpf elemek létrehozásánál mindenképp jelezni kell majd, hogy melyik kérédshez tartozik.
            /// Ezt a QuizContents táblából kiolvasott rekordok Id-vel lehetne megoldani, úgy hogy az adott wpf elem name vagy egyéb property-ének az Id-t adjuk értékül.
            /// így a kiértékelésnél a wpf elemek name property-ét kellene nézni.
            /// 
            /// A kérdések kiválasztását az adatbázis QuizContents táblájából véletlenszerűen kellene megoldani.
            /// Lehetséges megoldás1: 
            ///     1. Lekérni a QuizContents táblából az összes Id-t, amit egy listába helyezünk.
            ///     2. A listaelemek sorrendjét összekeverni és az első N elemet használjuk fel, függően mennyi kérdést akarunk egy körben feltenni.
            ///        Az összekeveréshez a Fisher-Yates algoritmust lehetne használni. (pl.: https://stackoverflow.com/questions/273313/randomize-a-listt vagy
            ///        https://stackoverflow.com/questions/273313/randomize-a-listt)
            ///        
            /// Lehetséges megoldás2:
            ///     1. Lekérni a QuizContents táblából az összes Id-t, amit egy listába helyezünk.
            ///     2. Létrehozunk egy másik üres listát.
            ///     3. Véletlenszám generátorral kiválasztunk elemeket az eredeti listából és áthelyezzük az üres listába. Mindezt annyiszor hajtjuk végre,
            ///        ahány kérdést akarunk feltenni.
            ///        
            /// Ezenkívül amikor kérédésekhez tartozó wpf elemeket kiírjuk, akkor a válaszok sorrendjét össze kell keverni, mivel a QuizContents táblából az alábbi
            /// sorrendben fogja lehívni az adatokat: Id, Question, GoodAnswer, WrongAnswer1, WrongAnswer2.
            /// Itt szerintem egy egyszerű véletlenszám generálással ezt meg lehet oldani.
        }
    }
}
