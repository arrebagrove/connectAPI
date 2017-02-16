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
using MySql.Data;
using MySql.Data.MySqlClient;

namespace ConnectApi
{
    class Program
    {
        // Your application access token to ConnectApi, AccountsApi Spotware
        // public static string accessToken = "";
        // public static string accessToken = "";
        public static string accessToken = "";
        // Save errors to logs
        public static isError err =  new isError();

        static void Main(string[] args)
        {
            if (!BreakermindForex.CreateDatabase())
            {                                
                return;                
            }            
            
            string urlAccounts = "https://api.spotware.com/connect/tradingaccounts?access_token="+accessToken;
            string cAccounts = "";
            // string urlData = @"{'data':[{'positionId':7980418,'entryTimestamp':1487151223210,'utcLastUpdateTimestamp':1487151223210,'symbolName':'GBPUSD','tradeSide':'BUY','entryPrice':1.2428,'volume':100000,'stopLoss':null,'takeProfit':null,'profit':-338,'profitInPips':-35.7,'commission':-3,'marginRate':1.17765,'swap':0,'currentPrice':1.23923,'comment':null,'channel':'cAlgo','label':null}]}";

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
                                string txt = "\r\n" + "Account " + acc.accountId + " Broker " + acc.brokerName + " Account Number " + acc.accountNumber;
                                WebClient pos = new WebClient();
                                string cAccountPositions = pos.DownloadString("https://api.spotware.com/connect/tradingaccounts/" + acc.accountId + "/positions?access_token=" + accessToken);
                                Console.WriteLine(txt + "\nPositions from account " + acc.accountId + " >>> " + cAccountPositions);
                                
                                // Deserialize to object class cTraderPositions
                                cTraderPositions positions = JsonConvert.DeserializeObject<cTraderPositions>(cAccountPositions);
                                foreach (cTraderPosition p in positions.data)
                                {
                                    // Add position to database
                                    Int32 Timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                                    BreakermindForex.PositionAdd(acc.accountId, acc.accountNumber, p.positionId, p.entryTimestamp, p.utcLastUpdateTimestamp, p.symbolName, p.tradeSide, p.entryPrice, p.volume, p.stopLoss, p.takeProfit, p.profit, p.profitInPips, p.commision, p.marginRate, p.swap, p.currentPrice, p.comment, p.chanel, p.label, BreakermindForex.GetIPAddress(),Timestamp.ToString());
                                }
                                // log to file
                                err.SavePositions(DateTime.UtcNow.ToString() + "  " + txt + "\nPositions from account " + acc.accountId + " >>> " + cAccountPositions);
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(DateTime.UtcNow.ToString() + "Coś nie tak z pobieraniem pozycji \r\n"+ee);
                                // log to file
                                err.SaveError(ee.ToString());
                            }
                        });
                        thread.Start();
                        Thread.Sleep(2002);
                    }                                        
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
