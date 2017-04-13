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
        const string urlWynikiLotto = "http://megalotto.pl/wyniki/lotto";
        const string urlWynikiPlus = "http://megalotto.pl/lotto-plus/wyniki";
        const string urlWygraneLotto = "http://megalotto.pl/wyniki/lotto/wygrane-z-dnia-";
        const string urlWygraneLottoPlus = "http://megalotto.pl/lotto-plus/wyniki/wygrane-z-dnia-";
        #endregion
        public DateTime Data;
        RodzajGry rodzajGry;
        public List<string> ListaWylosowanych;

        public bool[] tablicaPoprawnych; // służy do określenia, które pozycje są trafione i mają być podświetlone
        public int LiczbaTrafionych { get; private set; }
        public string[] LiczbaWygranych { get; private set; }
        public string[] WysokoscWygranych { get; private set; }
        public string[] NazwyTrafien { get; private set; }
            
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
            // w zależności od wyboru rodzaju gry, przypisujemy adresUrl do zmiennej url
            switch (rodzajGry)
            {
                case RodzajGry.Lotto:
                    url = urlWynikiLotto;
                    break;
                case RodzajGry.LottoPlus:
                    url = urlWynikiPlus;
                    break;
                default:
                    url = "";
                    break;
            }
            ListaWylosowanych = new List<string>();
            var web = new HtmlWeb();
            HtmlNode dane;
            var html = web.Load(url);
            
            for (int i = 1; i < (int)rodzajGry; i++)
            {
                // XPath ze strony do wyników Duży Lotek  //*[@id="middle"]/div/div[2]/div/ul[1]/li[2]   -   http://megalotto.pl/wyniki/lotto
                dane = html.DocumentNode.SelectSingleNode("//*[@id='middle']/div/div[2]/div/ul[" + i + "]/li[2]");
                Console.WriteLine(Data.ToString("dd-MM-yyyy"));
                if (dane.InnerText == Data.ToString("dd-MM-yyyy"))
                {
                    for (int j = 0; j < 6; j++)
                    {
                        ListaWylosowanych.Add(html.DocumentNode.SelectSingleNode("//*[@id='middle']/div/div[2]/div/ul[" + i + "]/li[" + (j + 3) + "]").InnerText.Trim());
                        Console.WriteLine(ListaWylosowanych[j]);
                    }
                }
            }            
        }
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
            switch (rodzajGry)
            {
                case RodzajGry.Lotto:
                    url = urlWygraneLotto;
                    break;
                case RodzajGry.LottoPlus:
                    url = urlWygraneLottoPlus;
                    break;
                //case RodzajGry.LottoMini:
                //    break;
                //case RodzajGry.MultiMulti:
                //    break;
                //case RodzajGry.EkstraPensja:
                //    break;
                default:
                    url = "";
                    break;
            }
            WysokoscWygranych = new string[4];
            LiczbaWygranych = new string[4];
            NazwyTrafien = new string[4];
            var webDok = new HtmlWeb();            
            string tekst;
            
            try
            {
                var html = webDok.Load(url + Data.ToString("dd-MM-yyyy"));
                tekst = html.DocumentNode.SelectSingleNode("//*[@id='wygrane_dla_archiwalnego_losowania']/div/table/tr[3]/td[2]").InnerText;
                for (int i = 0; i < 4; i++)
                {
                    WysokoscWygranych[i] = html.DocumentNode.SelectSingleNode("//*[@id='wygrane_dla_archiwalnego_losowania']/div/table/tr[" + (i + 2) + "]/td[3]").InnerText;
                    LiczbaWygranych[i] = html.DocumentNode.SelectSingleNode("//*[@id='wygrane_dla_archiwalnego_losowania']/div/table/tr[" + (i + 2) + "]/td[2]").InnerText;
                    NazwyTrafien[i] = html.DocumentNode.SelectSingleNode("//*[@id='wygrane_dla_archiwalnego_losowania']/div/table/tr[" + (i + 2) + "]/td[1]").InnerText;
                }
            }
            catch (Exception ex)
            {
                WysokoscWygranych[0] = "Brak danych za ten okres";
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
