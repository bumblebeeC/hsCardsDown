using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using Newtonsoft.Json;
namespace Getcards
{
    class Page
    {
        int prePage { get; set; }
        int total { get; set; }
        string curCardClass { get; set; }
        public int nextPage { get; set; }
        int curPage { get; set; }
        int pageSize { get; set; }
        int totalPage { get; set; }
        public  List<Cards> cards { get; set; }
        public Page()
        {
            cards = new List<Cards>();
        }
    }

    class Cards {
        public int id { get; set; }
        public string name { get; set; }
        string code { get; set; }
        public string description { get; set; }
        public string background { get; set; }
        public string imageUrl { get; set; }
        public string cost { get; set; }
        public string attack { get; set; }
        public string health { get; set; }
    }
    class Program
    {  
        public static string Post(string url, string param)
        {
            string strURL = url;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "POST";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.Headers.Add("Cookie", " _ntes_nnid=e38f6dc76517797eb695858c28ebe75c,1460535410049; _ANTICSRF=eee3fcd4-7fb9-4dc0-83f1-0042a9cfed39");
            string paraUrlCoded = param;
            byte[] payload;
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            request.ContentLength = payload.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s;
            s = response.GetResponseStream();
            string StrDate = "";
            string strValue = "";
            StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            while ((StrDate = Reader.ReadLine()) != null)
            {
                strValue += StrDate + "\r\n";
            }
            return strValue;
        }

        public static string GetPostData(string cardclass ,int nextpage) {
            return  "cardClass=" + cardclass + "&p=" + nextpage + "&golden=0&t=1478159795341&token=eee3fcd4-7fb9-4dc0-83f1-0042a9cfed39";
        }

        static string url = "http://hs.blizzard.cn/cards/query";
        static int NextPage = 1;
        

        static void Main(string[] args)
        {
          
            string[] cardList = { "druid", "hunter", "mage", "neutral", "paladin", "priest", "rogue", "shaman", "warlock", "warrior" };
            Page p = new Page();
            
            for (int i = 0; i < cardList.Length; i++)
            {
                Console.WriteLine(cardList[i]);
              p= JsonConvert.DeserializeObject<Page>(Post(url, GetPostData(cardList[i], NextPage)));
              //Console.WriteLine(p.cards.Count);
              NextPage = p.nextPage;
              while (NextPage != 1)
              {
                  for (int c = 0; c < p.cards.Count; c++)
                  {
                      Console.WriteLine(p.cards[c].id+" "+ p.cards[c].name+" "+p.cards[c].cost + " "+p.cards[c].attack+" "+p.cards[c].health +"\n"+p.cards[c].background + "\n"+p.cards[c].description+"\n"+p.cards[c].imageUrl);
                  }
                  p = JsonConvert.DeserializeObject<Page>(Post(url, GetPostData(cardList[i], NextPage)));
                  NextPage = p.nextPage;
              }
              
            }

            Console.ReadKey();

        
        }
    }
}
