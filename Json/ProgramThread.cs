using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConnectApi
{
    class Program
    {

        static void Main(string[] args)
        {
            
            string urlPositions = "https://api.spotware.com/connect/tradingaccounts/428599/positions?access_token=u_9jgKZevYqkg_DDYT5EkJA55Rnu29g7YdLotTNWTSM";
            string urlAccounts = "https://api.spotware.com/connect/tradingaccounts?access_token=qvhSLbgAAbtY9vIHz9Dtc_1KIB9EJw7xoUiQgQ2r5Ao";
            string url = @"{'data':[{'positionId':7980418,'entryTimestamp':1487151223210,'utcLastUpdateTimestamp':1487151223210,'symbolName':'GBPUSD','tradeSide':'BUY','entryPrice':1.2428,'volume':100000,'stopLoss':null,'takeProfit':null,'profit':-338,'profitInPips':-35.7,'commission':-3,'marginRate':1.17765,'swap':0,'currentPrice':1.23923,'comment':null,'channel':'cAlgo','label':null}]}";
            string cAccounts = "";

            while (true) {
                using (var wc = new WebClient())
                {
                    cAccounts = wc.DownloadString(urlAccounts);
                    Console.WriteLine(cAccounts);
                }

                // Deserialize to object class cTraderAccounts
                cTraderAccounts result = JsonConvert.DeserializeObject<cTraderAccounts>(cAccounts);
                // get first account id
                var aId = result.data[0].accountId;
                Console.WriteLine(aId);

                // show all accounts data
                foreach (cTraderAccount acc in result.data)
                {
                    Thread thread = new Thread(()=>
                    {
                        Console.WriteLine("Account " + acc.accountId + " Broker " + acc.brokerName);
                        WebClient pos = new WebClient();
                        string cAccountPositions = pos.DownloadString("https://api.spotware.com/connect/tradingaccounts/" + acc.accountId + "/positions?access_token=qvhSLbgAAbtY9vIHz9Dtc_1KIB9EJw7xoUiQgQ2r5Ao");
                        Console.WriteLine("Positions from account " + acc.accountId + " >>> " + cAccountPositions);
                    });
                    thread.Start();
                }

               // Thread.Sleep(2000);
                // Console.ReadKey();
            }
        }
               

    }
}
