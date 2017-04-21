using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;


namespace Lotek
{
    class GraLiczbowa
    {
#region stałe adresy        
        //TODO: wprowadzić w wynikach możliwość zmiany roku
        const string urlWynikiLotto = "http://megalotto.pl/lotto/wyniki/losowania-z-roku-";
        const string urlWygraneLotto = "http://megalotto.pl/lotto/wygrane/";
        const string urlWynikiPlus = "http://megalotto.pl/lotto-plus/wyniki/losowania-z-roku-";
        const string urlWygraneLottoPlus = "http://megalotto.pl/lotto-plus/wygrane/";
        const string urlWynikiMini = "http://megalotto.pl/mini-lotto/wyniki/losowania-z-roku-";
        const string urlWygraneMini = "http://megalotto.pl/mini-lotto/wygrane/";

        #endregion
        public DateTime Data;
        RodzajGry rodzajGry;
        public List<string> ListaWylosowanych;
        public string Informacja { get; private set; }

        public bool[] tablicaPoprawnych; // służy do określenia, które pozycje są trafione i mają być podświetlone
        public int LiczbaTrafionych { get; private set; } // liczba trafionych liczb
        public List<string> IloscWygranych { get; private set; } // liczba trafionych wygranych danego stopnia
        public List<string> WysokoscWygranej { get; private set; } // wysokość wygranej dla poszczególnych ilości trafień
        public List<string> NazwyTrafien { get; private set; } // słowna nazwa ilości trafień ( pobrana ze strony )
            
        public GraLiczbowa(DateTime data, RodzajGry rodzaj)
        {
            this.Data = data;
            this.rodzajGry = rodzaj;
            PobierzDane();
            PobierzWygraneLotto(Data, rodzajGry);
        }                   
        public void PobierzDane()
        {
            string url;
            int liczbaLosowanychLiczb = 0;
            // w zależności od wyboru rodzaju gry, przypisujemy adresUrl do zmiennej url oraz liczbę losowanych liczb
            switch (rodzajGry)
            {
                case RodzajGry.Lotto:
                    url = urlWynikiLotto;
                    liczbaLosowanychLiczb = 6;
                    break;
                case RodzajGry.LottoPlus:
                    url = urlWynikiPlus;
                    liczbaLosowanychLiczb = 6;
                    break;
                case RodzajGry.LottoMini:
                    url = urlWynikiMini;
                    liczbaLosowanychLiczb = 5;
                    break;
                default:
                    url = "";
                    break;
            }
            ListaWylosowanych = new List<string>();
            var web = new HtmlWeb();
            HtmlNode dane;
            var html = web.Load(url+Data.Year);

            //pobiera tablicę losowań
            dane = html.DocumentNode.SelectSingleNode("//*[@id='middle']/div/div[2]/div");
            // ustawia liczbę widocznych losowań pobranych ze strony
            int iloscWidocznychLosowan = dane.ChildNodes.Count;
            for (int i = 1; i < iloscWidocznychLosowan; i++)
            {
                //wskazuje datę w tablicy losowań
                dane = html.DocumentNode.SelectSingleNode("//*[@id='middle']/div/div[2]/div/ul[" + i + "]/li[2]"); 
                // jeżeli powyższa data zgadza się z datą wybraną przez użytkownika
                if (dane.InnerText == Data.ToString("dd-MM-yyyy"))
                {
                    for (int j = 0; j < liczbaLosowanychLiczb; j++)
                    {
                        // dodaj liczby z określonej daty do listy wylosowanych, bez zbędnych znaków
                        ListaWylosowanych.Add(html.DocumentNode.SelectSingleNode
                            ("//*[@id='middle']/div/div[2]/div/ul[" + i + "]/li[" + (j + 3) + "]")
                            .InnerText.Trim()); 
                    }
                    break;
                }                
            }
            if (ListaWylosowanych.Count == 0)
            {
                for (int k = 0; k < liczbaLosowanychLiczb; k++)
                {
                    ListaWylosowanych.Add("...");
                }
            }
        }
        /// <summary>
        /// Tworzy tablicę z pozycjami trafionymi w celu wykorzystani przy określaniu pozycji trafionych
        /// </summary>
        /// <param name="wybraneLiczby">wprowadź tablicę liczb</param>
        public void PobierzTrafione(string[] wybraneLiczby)
        {
            LiczbaTrafionych = 0;
            tablicaPoprawnych = new bool[] { false, false, false, false, false, false };
            for (int i = 0; i < ListaWylosowanych.Count; i++)
            {
                for (int j = 0; j < ListaWylosowanych.Count; j++)
                {
                    if (ListaWylosowanych[i].ToString() == wybraneLiczby[j])
                    {
                        tablicaPoprawnych[i] = true;
                        LiczbaTrafionych++;
                    }
                }
            }
        }
        public void PobierzWygraneLotto(DateTime data, RodzajGry rodzajGry)
        {
            string url;
            int stopniWygranych = 0;
            switch (rodzajGry)
            {
                case RodzajGry.Lotto:
                    url = urlWygraneLotto;
                    break;
                case RodzajGry.LottoPlus:
                    url = urlWygraneLottoPlus;
                    break;
                case RodzajGry.LottoMini:
                    url = urlWygraneMini;
                    break;
                //case RodzajGry.MultiMulti:
                //    break;
                //case RodzajGry.EkstraPensja:
                //    break;
                default:
                    url = "";
                    break;
            }
            WysokoscWygranej = new List<string>();
            IloscWygranych = new List<string>();
            NazwyTrafien = new List<string>();

            var webDok = new HtmlWeb();            
            HtmlNode nodeTekst;

            try
            {
                var html = webDok.Load(url + Data.ToString("dd-MM-yyyy"));
                // pobierz tablicę z wygranymi
                nodeTekst = html.DocumentNode.SelectSingleNode("//*[@id='middle']/div/div/table");
                // wyciąg liczbę pozycji - 1 co daje ilość stopni wygranych

                if (nodeTekst != null)
                {
                    stopniWygranych = nodeTekst.ChildNodes.Count - 1;

                    for (int i = 0; i < stopniWygranych; i++)
                    {
                        WysokoscWygranej.Add ( html.DocumentNode.SelectSingleNode("//*[@id='middle']/div/div/table/tr[" + (i + 2) + "]/td[3]").InnerText);
                        IloscWygranych.Add ( html.DocumentNode.SelectSingleNode("//*[@id='middle']/div/div/table/tr[" + (i + 2) + "]/td[2]").InnerText);
                        NazwyTrafien.Add ( html.DocumentNode.SelectSingleNode("//*[@id='middle']/div/div/table/tr[" + (i + 2) + "]/td[1]").InnerText);
                    }
                }
                //else
                //{
                //    WysokoscWygranej.Add("Brak danych");
                //    IloscWygranych.Add("na wskazany");
                //    NazwyTrafien.Add("okres");
                //}
                
        }
            catch (Exception e)
            {
                Informacja = "Błąd podczas pobierania danych z wysokością wygranych \n" + e.Message;
                System.Windows.Forms.MessageBox.Show(e.Message);
            }

}

        
        //const string adresUrl = "http://app.lotto.pl/wyniki/?type=dl";
        //const string adresWygrane = "http://app.lotto.pl/wygrane/?type=dl";


        //public void PobierzWygraneLotto()
        //{
        //    WebClient klientWeb = new WebClient();
        //    string[] daneZserwisu = klientWeb.DownloadString(adresWygrane).Split('\n');
        //    StopnieWygranych = new string[4];
        //    WysokoscWygranych = new string[4];
        //    testWygrane = new string[10, 10];
        //    for (int i = 0; i < 4; i++)
        //    {
        //        string[] tempTablica;
        //        tempTablica = daneZserwisu[i + 1].Split('\t');
        //        for (int j = 0; j < 4; j++)
        //        {
        //            StopnieWygranych[i] = tempTablica[0];
        //            WysokoscWygranych[i] = tempTablica[1];
        //        }
        //    }
        //}




        //void PobierzDzisiejszeDane()
        //{
        //    WebClient klientWeb = new WebClient();
        //    string daneZserwisu = klientWeb.DownloadString(adresUrl);
        //    string[] tablica = daneZserwisu.Split('\n');
        //    data = tablica[0];
        //    for (int i = 1; i < 7; i++)
        //    {
        //        listaWylosowanych.Add(Convert.ToInt32(tablica[i]));
        //    }
        //    listaWylosowanych.Sort();
        //    PobierzWygraneLotto();
        //}
        //public List<int> PobierzWylosowane()
        //{
        //    return listaWylosowanych;           
        //}



    }
}
