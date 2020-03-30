using kviz_jatek.Data;
using kviz_jatek.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System;

namespace kviz_jatek
{
    /// <summary>
    /// QuizWindow.xaml
    /// Ebben az ablakban jelennek meg az adatbázisból véletlenszerűen kiválasztott kérdések és a hozzájuk tartozó (jó-rossz)válaszok
    /// A kitöltés után a válaszok kiértékelésre kerülnek és függően a pontszámtól felkerül a játékos a legjobb pontszámok táblázatába
    /// ide vaalmit beleírunk bala
    /// Tehát mégegyszer módosítunk
    /// </summary>
    public partial class QuizWindow : Window
    {
        private readonly Window mainwindow;         // referencia a főmenühöz
        private readonly DatabaseContext context;   // referencia az adatbázis eléréséhez

        int db_kerdesek_szama = 0;                 ///Az összes kérdés száma a későbbiekben az adatbázisból lekérdezve
        int osszes_feltett_kerdes = 10;            ///A kvíz során feltett kérdések száma         
        Random veletlen_szam = new Random();
        List<int> eddigiek = new List<int>();      ///Az eddig feltett kérdések számlálója
        int jo_valaszok_szama = 0;                 ///A jó válaszok számlálója

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
        private void OnClick_GetNext(object sender, RoutedEventArgs e)
        {
            int i;
            List<QuizContent> questionlist = context.QuizContents.ToList();
            db_kerdesek_szama = questionlist.Count;         ///A kérdések darabszáma a véletlenszám generálás határaihoz

            if(eddigiek.Count == 0)
            {
                jo_valaszok_szama = 0;
            }

            if(eddigiek.Count != osszes_feltett_kerdes)
            {
                if (eddigiek.Count>0)
                {
                    int valasz = eddigiek[eddigiek.Count - 1];
                    if (valasz%3 == 0 && Valaszok.SelectedItem==Valasz0)
                    {
                        jo_valaszok_szama += 1;
                    }
                    else if (valasz % 3 == 1 && Valaszok.SelectedItem == Valasz1)
                    {
                        jo_valaszok_szama += 1;
                    }
                    else if (valasz % 3 == 2 && Valaszok.SelectedItem == Valasz2)
                    {
                        jo_valaszok_szama += 1;
                    }
                }
                do
                {
                    i = veletlen_szam.Next(0, db_kerdesek_szama);
                }
                while (eddigiek.Contains(i));

                eddigiek.Add(i);

                Kovetkezo.Content = "Következő";
                Valaszok.Visibility = Visibility.Visible;
                Valaszok.SelectedValue = "none";
                Kerdes.Text = questionlist[i].Question;

                ///A jó válasz helye attól függ, hogy a véletlenszám 3-mal osztva mennyit ad maradékul
                ///Nem a legelegánsabb, de egyszerűbben kezelhető
                if (i % 3 == 0)
                {
                    Valasz0.Content = questionlist[i].GoodAnswer;
                    Valasz1.Content = questionlist[i].WrongAnswer1;
                    Valasz2.Content = questionlist[i].WrongAnswer2;
                }
                else if (i % 3 == 1)
                {
                    Valasz0.Content = questionlist[i].WrongAnswer1;
                    Valasz1.Content = questionlist[i].GoodAnswer;
                    Valasz2.Content = questionlist[i].WrongAnswer2;
                }
                else
                {
                    Valasz0.Content = questionlist[i].WrongAnswer1;
                    Valasz1.Content = questionlist[i].WrongAnswer2;
                    Valasz2.Content = questionlist[i].GoodAnswer;
                }

            }
            else
            {
                Kovetkezo.Visibility = Visibility.Collapsed;
                Valaszok.Visibility = Visibility.Collapsed;
                Kerdes.Text = "Eredmény: " + jo_valaszok_szama + "/" + osszes_feltett_kerdes + " = " + 100*jo_valaszok_szama/osszes_feltett_kerdes + "%";
                
                ///80%-nál jobb eredmény rögzíthető
                if (jo_valaszok_szama/osszes_feltett_kerdes > 0.8)
                {
                    Rogzites_Cimke.Visibility = Visibility.Visible;
                    Nev.Visibility = Visibility.Visible;
                    Rogzites.Visibility = Visibility.Visible;
                }
            }
            

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
            /*   List<QuizContent> questionlist = context.QuizContents.ToList();
               db_kerdesek_szama = questionlist.Count;         ///A kérdések darabszáma a véletlenszám generálás határaihoz

               int i = veletlen_szam.Next(0, db_kerdesek_szama);
               eddigiek.Add(i);
               Kerdes.Content = questionlist[i].Question;
              */

            eddigiek.Clear();
            //jo_valaszok_szama = 0;
            Kovetkezo.Content = "Indítás";
            Kovetkezo.Visibility = Visibility.Visible;
            Valaszok.Visibility = Visibility.Collapsed;
            Kerdes.Text = "A quiz indításához kattins az Indításra!";


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
            /// 
            /// 
            ///--------Laci----------------------------------------------------------------------------------------------------------------------------------------------------   
            /// Esetleg egy harmadik megoldási módszer lehetne:
            ///     A kérdéseket és válasz lehetőségeket nem egyszerre jelenítenénk meg, hanem egymás után ("Következő" gomb).
            ///     A válasz lehetőségek nem RadioButtonnal, hanem ComboBox-ban lennének, előzetesen összekeverve.
            ///     Így alapvetően elég lenne egy fix kialakítás a wpf elemekre, csak a tartalmukat kellene változtatni és ellenőrizni.
            ///     Egy gomb megnyomásával lehetne a következő kérdésre menni, és az adott kérdés kiértékelése már ekkor megtörténne az alapján, hogy az aktuális kérdéshez tartozó 
            ///     jó válasz lett-e kiválasztva a ComboBox-ban. Mivel az adatbázisból származik a kérdés és a válaszok is, egy lekérdezéssel megálapítható, hogy a jó válasz lett-e kiválasztva.
            ///     Így nem lenne szükség egy külön kiértékelés függvényre, hanem egy ciklusban végig lehetne pörgetni a dolgot.
            ///     Visszalépésre nem lenne lehetőség az egyszerűség kedvééért.
            ///     Jó válaszok esetén kellene léptetni egy számlálót.
            ///     
            ///     Az egyszerűség kedvéért lehet egy általunk meghatározott fix számú kérdés, vagy lehet a felszanáló által választott mennyiség is, bár ez utóbbi esetben kell még egy lépés
            ///     vagy felület, ahol ezt a számot megadja. Ennek a számnak és az összesített jó válaszok számának az aránya megadja az eredményt is.
        }

        private void OnClick_Rogzites(object sender, RoutedEventArgs e)
        {

        }
    }
}
