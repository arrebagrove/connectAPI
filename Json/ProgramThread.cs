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
        public static isError err =  new isError();
        static void Main(string[] args)
        {
            // string urlData = @"{'data':[{'positionId':7980418,'entryTimestamp':1487151223210,'utcLastUpdateTimestamp':1487151223210,'symbolName':'GBPUSD','tradeSide':'BUY','entryPrice':1.2428,'volume':100000,'stopLoss':null,'takeProfit':null,'profit':-338,'profitInPips':-35.7,'commission':-3,'marginRate':1.17765,'swap':0,'currentPrice':1.23923,'comment':null,'channel':'cAlgo','label':null}]}";
            string accessToken = "qvhSLbgAAbtY9vIHz9Dtc_1KIB9EJw7xoUiQgQ2r5Ao";
            string urlAccounts = "https://api.spotware.com/connect/tradingaccounts?access_token="+accessToken;
            string cAccounts = "";

            try
            {
               try { 
                    using (var wc = new WebClient())
                    {
                        // Get account from accessToken
                        cAccounts = wc.DownloadString(urlAccounts);
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine("Coś nie tak z pobieraniem kont");
                    // log error to file
                    err.SaveError(ee.ToString());
                }

                while (true)
                {
                    // Deserialize to object class cTraderAccounts
                    cTraderAccounts result = JsonConvert.DeserializeObject<cTraderAccounts>(cAccounts);                    
                    // var aId = result.data[0].accountId;

                    // show all accounts data
                    foreach (cTraderAccount acc in result.data)
                    {
                        Thread thread = new Thread(() =>
                        {
                            try
                            {
                                string txt = "\r\n" + "Account " + acc.accountId + " Broker " + acc.brokerName;
                                WebClient pos = new WebClient();
                                string cAccountPositions = pos.DownloadString("https://api.spotware.com/connect/tradingaccounts/" + acc.accountId + "/positions?access_token=" + accessToken);
                                Console.WriteLine(txt + "\nPositions from account " + acc.accountId + " >>> " + cAccountPositions);
                                // log to file
                                err.SavePositions(DateTime.UtcNow.ToString() + "  " + txt + "\nPositions from account " + acc.accountId + " >>> " + cAccountPositions);
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(DateTime.UtcNow.ToString() + "Coś nie tak z pobieraniem pozycji");
                                // log to file
                                err.SaveError(ee.ToString());
                            }
                        });
                        thread.Start();
                    }
                    Thread.Sleep(2000);                  
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Spotwer error : " + e.ToString());
                // log to file
                err.SaveError(e.ToString());
            }
        }
    }
}
