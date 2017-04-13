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

        public Form1()
        {
            InitializeComponent();
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            Lotto = new GraLiczbowa(dateTimePicker1.Value, RodzajGry.Lotto);
            LottoPlus = new GraLiczbowa(dateTimePicker1.Value, RodzajGry.LottoPlus);

            WybraneLotto_textBox.Text = "10,6,37,44,41,13"; // przykładowe liczby
        }
        /// <summary>
        /// wyszukuje odpowiednie labele i wprowadza w etykiety odpowiednie wylosowane liczby
        /// </summary>
        /// <param name="czescNazwyLabela">podaj powtarzajacą się część nazwy etykiek do których mają zostać wprowadzone liczby</param>
        /// <param name="gra">gra liczbowa, której liczby mają zostać wyświetlone</param>
        void wprowadzWylosowaneLiczby(string czescNazwyLabela, GraLiczbowa gra)
        {
            
            for (int i = 0; i < gra.ListaWylosowanych.Count; i++)
            {
                foreach (Control label in tabPage3.Controls)
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
            for (int i = 0; i < 4; i++)
            {
                RBoxDoWyswietleniaDaanych.AppendText("Liczba trafionych " + gra.NazwyTrafien[i] + " to: " + gra.LiczbaWygranych[i] + "\n" + "Płacili: " + gra.WysokoscWygranych[i] + "\n");
            }
        }
        /// <summary>
        /// Podświetla etykiety liczb, które zostały trafione
        /// </summary>
        /// <param name="nazwaLabelaZawierajacaSlowo">podaj powtarzajacą się część nazwy etykiet, które mają zostać podświetlone</param>
        /// <param name="graLiczbowa">gra liczbowa, której wgrane mają się wyświetlić</param>
        /// <param name="liczbyUzytkownika_Tbox">tekst box w którym użytkownik podaje wybrane liczby</param>
        private void KolorujTrafione(string nazwaLabelaZawierajacaSlowo, GraLiczbowa graLiczbowa,TextBox liczbyUzytkownika_Tbox)
        {
            // ustaw tło labeli na bezbarwne
            string[] wybrane = liczbyUzytkownika_Tbox.Text.Split(',');
            foreach (Control label in tabPage3.Controls)
            {
                if (label.Name.Contains(nazwaLabelaZawierajacaSlowo))
                {
                    label.BackColor = Color.Empty;
                }                    
            }
            
            graLiczbowa.PobierzTrafione(wybrane);

            for (int i = 1; i < graLiczbowa.tablicaPoprawnych.Length+1; i++)
            {
                foreach (Control label in tabPage3.Controls)
                {
                    if (label.Name == nazwaLabelaZawierajacaSlowo + (i) + "_lbl")
                    {
                        if (graLiczbowa.tablicaPoprawnych[i - 1])
                            label.BackColor = Color.GreenYellow;
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                wprowadzWylosowaneLiczby("lotto", Lotto);
                wprowadzWylosowaneLiczby("plus", LottoPlus);
                KolorujTrafione("lotto", Lotto, WybraneLotto_textBox);
                KolorujTrafione("plus", LottoPlus, WybraneLotto_textBox);
                wypiszTrafieniaiEwentualneWygrane(Lotto,informacja_label);
                wypiszTrafieniaiEwentualneWygrane(LottoPlus, lbl_wygrane_Plus);
                uzupelnijWygrane(Lotto, WygraneLotto_richTextBox);
                uzupelnijWygrane(LottoPlus, WygraneLottoPlus_RBox);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wprowadź poprawnie dane, np: \n 1,5,10,18,24,39\n" + ex.Message, "Wprowadzono niepoprawne dane.");
            }
        }
        /// <summary>
        /// Funkcja sprawdza trafienia i wypisuje komunikat ile liczb trafiono i ewentualną wygraną
        /// </summary>
        /// <param name="gra">obiekt gra liczbowa, który ma zostać sprawdzony</param>
        /// <param name="labelNainformacje">label w którym informajce mają być wypisane</param>
        void wypiszTrafieniaiEwentualneWygrane(GraLiczbowa gra, Label labelNainformacje)
        {
            string wysokoscWygranej;
            switch (gra.LiczbaTrafionych)
            {
                case 3:
                    wysokoscWygranej = gra.WysokoscWygranych[3];
                    break;
                case 4:
                    wysokoscWygranej = gra.WysokoscWygranych[2];
                    break;
                case 5:
                    wysokoscWygranej = gra.WysokoscWygranych[1];
                    break;
                case 6:
                    wysokoscWygranej = gra.WysokoscWygranych[0];
                    break;
                default:
                    wysokoscWygranej = "Nic nie wygrałeś !!! :(";
                    break;
            }
            labelNainformacje.Text = "Trafiłeś " + gra.LiczbaTrafionych + " cyfry.\n" +
                "Wygrałeś: " + wysokoscWygranej + " złotych.";
        }
    }
}
