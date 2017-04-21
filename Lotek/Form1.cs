using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lotek
{
    public partial class Form1 : Form
    {
        GraLiczbowa Lotto;
        GraLiczbowa LottoPlus;
        GraLiczbowa Mini;
        //TODO: Stworzyć archiwum lokalne, aktualizowane o nowe pozycje podczas uruchomiania
        public Form1()
        {
            InitializeComponent();
        }
        #region TabPage_Enter
        private void tabPage_Lotto_Enter(object sender, EventArgs e)
        {
            txtBox_wybrane_Lotto.Text = "10,6,37,44,41,13"; // przykładowe liczby
            btn_sprawdz_Lotto.PerformClick();
        }
        private void tabPage_MiniLotto_Enter(object sender, EventArgs e)
        {
            txtBox_wybrane_Mini.Text = "10,6,37,44,41"; // przykładowe liczby
            btn_sprawdz_Mini.PerformClick();
        }
        #endregion

        #region Buttony
        private void btn_sprawdz_Lotto_Click(object sender, EventArgs e)
        {
            Sprawdz(Lotto, RodzajGry.Lotto, DataTimePicker_Ogol, "lotto", 
                txtBox_wybrane_Lotto, lbl_wygrane_lotto, RichTextBox_wygrane_Lotto, tabPage_Lotto);
            Sprawdz(LottoPlus, RodzajGry.LottoPlus, DataTimePicker_Ogol, "plus", 
                txtBox_wybrane_Lotto, lbl_wygrane_Plus, RichTextBox_wygrane_Plus, tabPage_Lotto);
        }
        private void btn_sprawdz_Mini_Click(object sender, EventArgs e)
        {
            Sprawdz(Mini, RodzajGry.LottoMini, DataTimePicker_Ogol, "mini", txtBox_wybrane_Mini, lbl_wygrane_Mini, RichTextBox_wygrane_Mini, tabPage_MiniLotto);
        }
        #endregion

        #region Funkcje
        /// <summary>
        /// wyszukuje odpowiednie labele i wprowadza w etykiety odpowiednie wylosowane liczby
        /// </summary>
        /// <param name="czescNazwyLabela">podaj powtarzajacą się część nazwy etykiek do których mają zostać wprowadzone liczby</param>
        /// <param name="gra">gra liczbowa, której liczby mają zostać wyświetlone</param>
        void wprowadzWylosowaneLiczby(string czescNazwyLabela, GraLiczbowa gra, TabPage zakladka)
        {

            for (int i = 0; i < gra.ListaWylosowanych.Count; i++)
            {
                foreach (Control label in zakladka.Controls)
                {
                    if (label.Name == czescNazwyLabela + (i + 1) + "_lbl")
                        label.Text = gra.ListaWylosowanych[i].ToString();
                }
            }
        }
        /// <summary>
        /// uzpełnia RichtechBox o listę wygranych
        /// </summary>
        /// <param name="gra">gra liczbowa, której wgrane mają się wyświetlić</param>
        /// <param name="RBoxDoWyswietleniaDaanych">tekstBox w którym mają się wyświetlić dane</param>
        void uzupelnijWygrane(GraLiczbowa gra, RichTextBox RBoxDoWyswietleniaDaanych)
        {
            RBoxDoWyswietleniaDaanych.Clear();
            if (gra.WysokoscWygranej.Count > 1)
            {
                for (int i = 0; i < gra.WysokoscWygranej.Count; i++)
                {
                    RBoxDoWyswietleniaDaanych.AppendText("Liczba trafionych " + gra.NazwyTrafien[i] + " to: " + gra.IloscWygranych[i] + "\n" + "Płacili: " + gra.WysokoscWygranej[i] + "\n");
                }
            }
            else
            {
                RBoxDoWyswietleniaDaanych.Text = "Brak danych \nna podany \nokres ";
            }            
        }
        /// <summary>
        /// Podświetla etykiety liczb, które zostały trafione
        /// </summary>
        /// <param name="nazwaLabelaZawierajacaSlowo">podaj powtarzajacą się część nazwy etykiet, które mają zostać podświetlone</param>
        /// <param name="graLiczbowa">gra liczbowa, której wgrane mają się wyświetlić</param>
        /// <param name="liczbyUzytkownika_Tbox">tekst box w którym użytkownik podaje wybrane liczby</param>
        private void KolorujTrafione(string nazwaLabelaZawierajacaSlowo, GraLiczbowa graLiczbowa, TextBox liczbyUzytkownika_Tbox, TabPage zakladka)
        {
            // ustaw tło labeli na bezbarwne
            string[] wybrane = liczbyUzytkownika_Tbox.Text.Split(',');
            foreach (Control label in zakladka.Controls)
            {
                if (label.Name.Contains(nazwaLabelaZawierajacaSlowo))
                {
                    label.BackColor = Color.Empty;
                }
            }
            // podświetlanie trafionych
            graLiczbowa.PobierzTrafione(wybrane);

            for (int i = 1; i < graLiczbowa.tablicaPoprawnych.Length + 1; i++)
            {
                foreach (Control label in zakladka.Controls)
                {
                    if (label.Name == nazwaLabelaZawierajacaSlowo + (i) + "_lbl")
                    {
                        if (graLiczbowa.tablicaPoprawnych[i - 1])
                            label.BackColor = Color.GreenYellow;
                    }
                }
            }
        }
        /// <summary>
        /// Funkcja sprawdza trafienia i wypisuje komunikat ile liczb trafiono i ewentualną wygraną
        /// </summary>
        /// <param name="gra">obiekt gra liczbowa, który ma zostać sprawdzony</param>
        /// <param name="labelNainformacje">label w którym informajce mają być wypisane</param>
        void wypiszTrafieniaiEwentualneWygrane(GraLiczbowa gra, Label labelNainformacje)
        {
            string wysokoscWygranej = "0";
            
            if ((gra.ListaWylosowanych[0] == "..."))
            {
                labelNainformacje.Text = "Brak losowania w danym dniu.";
            }
            else if (gra.WysokoscWygranej.Count != 0)
            {
                // wartosc bezwzględna różnicy między liczbą trafionych a liczbą wylosowanych
                // daje nam to stopień wygranej ( 0 to najwyższa itd ), który można przyporządkować do tablicy wysokoscWygranych
                int roznica = Math.Abs(gra.LiczbaTrafionych - gra.ListaWylosowanych.Count);
                // jeżeli różnica da się przyporządkować do tablicy wygranych to...
                 if(roznica <= gra.WysokoscWygranej.Count)
                wysokoscWygranej = gra.WysokoscWygranej[roznica];
                labelNainformacje.Text = "Liczba trafień to: " + gra.LiczbaTrafionych + " cyfry.\n" +
                "Wygrana: " + wysokoscWygranej + " złotych.";
            }            
            else
            {
                wysokoscWygranej = "0";
                labelNainformacje.Text = "Liczba trafień to: " + gra.LiczbaTrafionych + " cyfry.\n" +
                "Wygrana: " + wysokoscWygranej + " złotych.";
            }            
        }
        /// <summary>
        /// zbiera wszystkie funkcje do kupy
        /// </summary>
        /// <param name="gra"></param>
        /// <param name="nazwaGry"></param>
        /// <param name="data"></param>
        /// <param name="czescNazwyEtykiety"></param>
        /// <param name="liczbywybrane"></param>
        /// <param name="etykietaWygrane"></param>
        /// <param name="listaWygranych"></param>
        void Sprawdz(GraLiczbowa gra, RodzajGry nazwaGry, DateTimePicker data, string czescNazwyEtykiety,
            TextBox liczbywybrane, Label etykietaWygrane, RichTextBox listaWygranych, TabPage zakladka)
        {
            gra = new GraLiczbowa(data.Value, nazwaGry);
            if (gra.ListaWylosowanych.Count != 0)
            {
                try
                {
                    wprowadzWylosowaneLiczby(czescNazwyEtykiety, gra, zakladka);
                    KolorujTrafione(czescNazwyEtykiety, gra, liczbywybrane, zakladka);
                    wypiszTrafieniaiEwentualneWygrane(gra, etykietaWygrane);
                    uzupelnijWygrane(gra, listaWygranych);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Wprowadź poprawnie dane\n" + e);
                }
            }
            else
            {
                listaWygranych.Text = "BRAK DANYCH\nZA TEN OKRES";
                etykietaWygrane.Text = "";
            }
            

        }

        #endregion

        
    }
}
