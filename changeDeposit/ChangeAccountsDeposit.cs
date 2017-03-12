using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using RestSharp;
using Newtonsoft.Json;
using System.Threading;
using System.IO;
// using MySql.Data.MySqlClient;

namespace ChangeAccountDeposit
{
    class Program
    {
        // Deposit change 
        // https://github.com/spotware/connect-csharp-web/blob/master/OpenApiDeveloperLibrary/Accounts/Transactions.cs
        // Download nuget.exe (https://dist.nuget.org/win-x86-commandline/latest/nuget.exe) 
        // and from command line add (http://restsharp.org/):
        // C:\> nuget.exe RestSharp 
        // C:\> nuget.exe Newtonsoft.Json
        // Add Reference to project and add:
        // using RestSharp;

        static void Main(string[] args)
        {
            string accessToken = "TOKENSTRING";

            // Change all accounts deposit to 1M (USD)
            string acc = ChangeAccountsDeposit(accessToken, 1000000);
            Console.WriteLine(acc);
            Console.ReadKey();
        }

        public static string ChangeAccountsDeposit(string accessToken, double initialDepositUSD = 1000000)
            {
            string urlAccounts = "https://api.spotware.com/connect/tradingaccounts?access_token=" + accessToken;
            string cAccounts = "";
            try
                {
                var wc = new WebClient();
                // Get account from accessToken
                cAccounts = wc.DownloadString(urlAccounts);
                dynamic accounts = JsonConvert.DeserializeObject<dynamic>(cAccounts);
                foreach (var item in accounts.data)
                    {
                    Console.WriteLine(item.accountId + " " + item.balance);
                    double balance = Double.Parse((string)item.balance) / 100;
                    Console.WriteLine("Balance " + balance);
                    if (balance > initialDepositUSD)
                        {
                        double amount = Math.Round(balance - initialDepositUSD, 2);
                        amount = amount * 100;
                        Withdraw(accessToken, (string)item.accountId, amount.ToString());
                        Console.WriteLine("Zmiejszam deposit " + amount);
                        }
                    if (balance < initialDepositUSD)
                        {
                        double amount = Math.Round(initialDepositUSD - balance, 2);
                        amount = amount * 100;
                        Deposit(accessToken, (string)item.accountId, amount.ToString());
                        Console.WriteLine("Zwiększam deposit " + amount);
                        }
                    }
                }
            catch (Exception ee)
                {
                Console.WriteLine("Coś nie tak z pobieraniem kont" + ee.ToString());
                }
            return cAccounts;
            }

        public static string GetDeals(string accountId, string accessToken, Int32 HistoryDays = 3)
            {
            WebClient pos = new WebClient();
            Int64 fromTimestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            fromTimestamp = fromTimestamp - (60 * 60 * 24 * HistoryDays);
            fromTimestamp = fromTimestamp * 1000;
            try
                {
                string Deals = pos.DownloadString("https://api.spotware.com/connect/tradingaccounts/" + accountId + "/deals?access_token=" + accessToken + "&fromTimestamp=" + fromTimestamp);
                return Deals;
                }
            catch (Exception e)
                {
                Console.WriteLine("Error account");
                return "0";
                }
            }

        public static string Deposit(string accessToken, string accountId, string amount)
            {
            string apiUrl = "https://api.spotware.com";
            var client = new RestClient(apiUrl);
            var request = new RestRequest(@"/connect/tradingaccounts/" + accountId + "/deposit/?oauth_token=" + accessToken, Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { amount = amount });
            return client.Execute(request).StatusDescription;
            }

        public static string Withdraw(string accessToken, string accountId, string amount)
            {
            string apiUrl = "https://api.spotware.com";
            var client = new RestClient(apiUrl);
            var request = new RestRequest(@"/connect/tradingaccounts/" + accountId + "/withdraw/?oauth_token=" + accessToken, Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { amount = amount });
            return client.Execute(request).StatusDescription;
            }
    }
}
