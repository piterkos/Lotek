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
        string data = "2017-04-04";
        List<string> listaWylosowanych = new List<string>();
        //List<int> listaWybranych;
        string adresUrl = "http://app.lotto.pl/wyniki/?type=dl";

        public GraLiczbowa()
        {
            PobierzDzisiejszeDane();
        }

        //public GraLiczbowa(string data)
        //{
        //    this.data = data;
        //    //this.listaWybranych = wybrane;
        //}

        void PobierzDzisiejszeDane()
        {
            WebClient klientWeb = new WebClient();
            string daneZserwisu = klientWeb.DownloadString(adresUrl);
            string[] tablica = daneZserwisu.Split('\n');
            data = tablica[0];
            for (int i = 1; i < 7; i++)
            {
                listaWylosowanych.Add(tablica[i]);
            }
            listaWylosowanych.Sort();
        }
        public List<string> PobierzWylosowane()
        {
            return listaWylosowanych;           
        }
    }
}
