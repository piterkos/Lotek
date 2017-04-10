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
        GraLiczbowa gra;
        List<string> wybrane;

        public Form1()
        {
            InitializeComponent();
        }
      
        private void tabPage3_Enter(object sender, EventArgs e)
        {
            gra = new GraLiczbowa();
            //Wygrane_richTextBox.Text = gra.PobierzWygraneLotto();
            for (int i = 0; i < gra.PobierzWylosowane().Count; i++)
            {
                foreach (Control label in tabPage3.Controls)
                {
                    if (label.Name == "lotto" + (i + 1) + "_lbl")
                        label.Text = gra.PobierzWylosowane()[i].ToString();
                }
            }
            for (int i = 0; i < 4; i++)
            {
                int stopien = 6;
                Wygrane_richTextBox.AppendText("Liczba trafionych " + (stopien - i) + " to: " + gra.StopnieWygranych[i] + "\n" + "Płacili: " + gra.WysokoscWygranych[i] + "\n");
            }
        }
        private void WprowadzWybrane()
        {
            string[] wybrane = WybraneLotto_textBox.Text.Split(',');
            gra.PobierzTrafione(wybrane);

            for (int i = 1; i < gra.tablicaPoprawnych.Length+1; i++)
            {
                foreach (Control label in tabPage3.Controls)
                {
                    if (label.Name == "lotto" + (i) + "_lbl")
                    {
                        //label.Text = wybrane[i-1];
                        if (gra.tablicaPoprawnych[i - 1])
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
                informacja_label.Text = "Trafiłeś " + gra.LiczbaTrafionych + " cyfry.\n" +
                    "Wygrałeś: " + wysokoscWygranej + " złotych.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wprowadź poprawnie dane, np: \n 1,5,10,18,24,39\n" + ex.Message, "Wprowadzono niepoprawne dane.")  ;
            }
            
        }
    }
}
