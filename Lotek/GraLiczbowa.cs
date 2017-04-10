using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Lotek
{
    class GraLiczbowa
    {
#region stałe adresy
        const string adresUrl = "http://app.lotto.pl/wyniki/?type=dl";
        const string adresWygrane = "http://app.lotto.pl/wygrane/?type=dl";
#endregion
        string data = "2017-04-04";
        List<int> listaWylosowanych = new List<int>();
        
        public bool[] tablicaPoprawnych = new bool[] { false, false, false, false, false, false };
        public int LiczbaTrafionych { get; private set; }
        public string[] StopnieWygranych { get; private set; }
        public string[] WysokoscWygranych { get; private set; }
        public string[,] testWygrane;



        public GraLiczbowa()
        {
            PobierzDzisiejszeDane();
        }
        void PobierzDzisiejszeDane()
        {
            WebClient klientWeb = new WebClient();
            string daneZserwisu = klientWeb.DownloadString(adresUrl);
            string[] tablica = daneZserwisu.Split('\n');
            data = tablica[0];
            for (int i = 1; i < 7; i++)
            {
                listaWylosowanych.Add(Convert.ToInt32(tablica[i]));
            }
            listaWylosowanych.Sort();
            PobierzWygraneLotto();
        }
        public List<int> PobierzWylosowane()
        {
            return listaWylosowanych;           
        }
        public void PobierzWygraneLotto()
        {
            WebClient klientWeb = new WebClient();
            string[] daneZserwisu = klientWeb.DownloadString(adresWygrane).Split('\n');
            StopnieWygranych = new string[4];
            WysokoscWygranych = new string[4];
            testWygrane = new string[10, 10];
            for (int i = 0; i < 4; i++)
            {
                string[] tempTablica;
                tempTablica = daneZserwisu[i+1].Split('\t');
                for (int j = 0; j < 4; j++)
                {
                    StopnieWygranych [i] = tempTablica[0];
                    WysokoscWygranych [i] = tempTablica[1];
                }
            }
        }
        public void PobierzTrafione(string[] wybraneLiczby)
        {
            for (int i = 0; i < listaWylosowanych.Count; i++)
            {
                for (int j = 0; j < listaWylosowanych.Count; j++)
                {
                    if (listaWylosowanych[i].ToString() ==wybraneLiczby[j])
                    {
                        tablicaPoprawnych[i] = true;
                        LiczbaTrafionych++;
                    }
                        
                }
            }            
        }
        
    }
}
