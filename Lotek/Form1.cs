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
        List<string> wybrane;

        public Form1()
        {
            InitializeComponent();
        }
      
        private void tabPage3_Enter(object sender, EventArgs e)
        {
            Lotto = new GraLiczbowa(dateTimePicker1.Value, RodzajGry.Lotto);
            LottoPlus = new GraLiczbowa(dateTimePicker1.Value, RodzajGry.LottoPlus);

            WybraneLotto_textBox.Text = "10,6,37,44,41,13"; // przykładowe liczby


            // wyszukuje odpowiednie labele i wprowadza w etykiety odpowiednie wylosowane liczby
            for (int i = 0; i < Lotto.listaWylosowanych.Count; i++)
            {
                foreach (Control label in tabPage3.Controls)
                {
                    if (label.Name == "lotto" + (i + 1) + "_lbl")
                        label.Text = Lotto.listaWylosowanych[i].ToString();
                }
            }
            for (int i = 0; i < LottoPlus.listaWylosowanych.Count; i++)
            {
                foreach (Control label in tabPage3.Controls)
                {
                    if (label.Name == "lotto" + (i + 7) + "_lbl")
                        label.Text = Lotto.listaWylosowanych[i].ToString();
                }
            }
            // uzpełnia RichtechBox o listę wygranych
            for (int i = 0; i < 4; i++)
            {
                Wygrane_richTextBox.AppendText("Liczba trafionych " + Lotto.NazwyTrafien[i] + " to: " + Lotto.LiczbaWygranych[i] + "\n" + "Płacili: " + Lotto.WysokoscWygranych[i] + "\n");
            }
            dateTimePicker1.Value = Convert.ToDateTime(Lotto.data);
        }
        private void WprowadzWybrane()
        {
            // ustaw tło labeli na bezbarwne
            foreach (Control label in tabPage3.Controls)
            {
                if (label.Name.Contains("lotto"))
                {
                    label.BackColor = Color.Empty;
                }                    
            }
            string[] wybrane = WybraneLotto_textBox.Text.Split(',');
            Lotto.PobierzTrafione(wybrane);

            for (int i = 1; i < Lotto.tablicaPoprawnych.Length+1; i++)
            {
                foreach (Control label in tabPage3.Controls)
                {
                    if (label.Name == "lotto" + (i) + "_lbl")
                    {
                        if (Lotto.tablicaPoprawnych[i - 1])
                            label.BackColor = Color.GreenYellow;
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                WprowadzWybrane();
                string wysokoscWygranej;
                switch (Lotto.LiczbaTrafionych)
                {
                    case 3:
                        wysokoscWygranej = Lotto.WysokoscWygranych[3];
                        break;
                    case 4:
                        wysokoscWygranej = Lotto.WysokoscWygranych[2];
                        break;
                    case 5:
                        wysokoscWygranej = Lotto.WysokoscWygranych[1];
                        break;
                    case 6:
                        wysokoscWygranej = Lotto.WysokoscWygranych[0];
                        break;
                    default:
                        wysokoscWygranej = "Nic nie wygrałeś !!! :(";
                        break;
                }
                informacja_label.Text = "Trafiłeś " + Lotto.LiczbaTrafionych + " cyfry.\n" +
                    "Wygrałeś: " + wysokoscWygranej + " złotych.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wprowadź poprawnie dane, np: \n 1,5,10,18,24,39\n" + ex.Message, "Wprowadzono niepoprawne dane.")  ;
            }
            
        }
    }
}
