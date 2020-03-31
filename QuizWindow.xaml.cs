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
            List<QuizContent> questionlist = context.QuizContents.ToList();     //A kérdéseket egy listába betöltjük, ahonnan választani fogunk
            db_kerdesek_szama = questionlist.Count;                             //A kérdések darabszáma a véletlenszám generálás határaihoz

            //A jó válaszok számlálóját az első kérdés előtt nullázzuk
            if(eddigiek.Count == 0)
            {
                jo_valaszok_szama = 0;
            }

            //A kapott válaszok helyességének ellenőrzése
            //Ha a legutóbbi kérdés azonosítója 3-mal osztva adott maradéka a TextBox megfelelő elémével egyezik, akkor jó válasznak könyvelhető
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

                //A következő kérdés nem lehet olyan, ami már korábban volt
                do
                {
                    i = veletlen_szam.Next(0, db_kerdesek_szama);
                }
                while (eddigiek.Contains(i));
                eddigiek.Add(i);

                //Az egyes elemek megjelenítése és tartalmuk változtatása
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
            //Amennyiben elértük a kérdéssor végét, kiírjuk az eredményt
            else
            {
                Kovetkezo.Visibility = Visibility.Collapsed;
                Valaszok.Visibility = Visibility.Collapsed;
                Kerdes.Text = "Eredmény: " + jo_valaszok_szama + "/" + osszes_feltett_kerdes + " = " + 100*jo_valaszok_szama/osszes_feltett_kerdes + "%";
                
                //80%-nál jobb eredmény rögzíthető
                if (100*jo_valaszok_szama/osszes_feltett_kerdes > 80)
                {
                    Rogzites_Cimke.Visibility = Visibility.Visible;
                    Nev.Visibility = Visibility.Visible;
                    Rogzites.Visibility = Visibility.Visible;
                }
            }
        }

        // Kérdések véletlen kiválasztása és kiírása
        private void OnActivated(object sender, System.EventArgs e)
        {

            eddigiek.Clear();                                   //Az eddig feltett kérdések Id-jait rögzítő lista ürítése
            
            //A megjelenített elemek kezdő étékeinek beállítása
            Kovetkezo.Content = "Indítás";
            Kovetkezo.Visibility = Visibility.Visible;
            Valaszok.Visibility = Visibility.Collapsed;
            Kerdes.Text = "A quiz indításához kattinson az Indításra!";

            //Az eredmény rögzítéshez szükséges elemek eltüntetésre kerülnek
            Rogzites_Cimke.Visibility = Visibility.Hidden;
            Nev.Visibility = Visibility.Hidden;
            Rogzites.Visibility = Visibility.Hidden;
        }

        private void OnClick_Rogzites(object sender, RoutedEventArgs e)
        {
            string nev = "Név nélkül";          //Ha nem adnak meg nevet, akkor "Név nélkül" névvel kerül rügzítésre az eredmény
            
            if (Nev.Text != "")
            {
                nev = Nev.Text;
            }
            
            ///Az eredmény adatbázisba rögzítése
            TopScore ujrekord = new TopScore
            {
                Name = nev,
                Score =100*jo_valaszok_szama/osszes_feltett_kerdes
            };
            context.TopScores.Add(ujrekord);
            context.SaveChanges();
            
            //Visszatérés a főmenü ablakához
            Hide();                 
            Thread.Sleep(150);      
            mainwindow.Show();      
        }
    }
}
