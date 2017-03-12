using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using RestSharp;

namespace DepositZero
{
    class SendWeb
    {
        public static string SendMsg(){
        
            return "1";
        }

        public static string Post(string url, string postData)
        {
            WebClient client = new WebClient();
            byte[] postArray = Encoding.ASCII.GetBytes(postData);
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            // client.Headers.Add("Content-Type", "application/json");
            byte[] responseArray = client.UploadData(url, postArray);
            return Encoding.ASCII.GetString(responseArray);
        }

        public static string UploadStringPost(string address, string data)
        {           
            WebClient client = new WebClient();        
            client.Encoding = System.Text.Encoding.UTF8;            
            string reply = client.UploadString(address, data);
            return reply;
        }
    }

 
    class Program
    {
        static void Main(string[] args)
        {

            // Deposit change 
            // https://github.com/spotware/connect-csharp-web/blob/master/OpenApiDeveloperLibrary/Accounts/Transactions.cs
            // Download nuget.exe (https://dist.nuget.org/win-x86-commandline/latest/nuget.exe) 
            // and from command line add (http://restsharp.org/):
            // C:\> nuget.exe RestSharp 
            // C:\> nuget.exe Newtonsoft.Json
            // Add Reference to project and add:
            // using RestSharp;

            string apiUrl = "https://api.spotware.com";
            string accessToken = "";
            string accountId = "123456";
            string amount = "500000";
            // change account deposit  
            string o = Deposit(apiUrl,accessToken,accountId,amount);
            Console.WriteLine(o);
            
            // Change account deposit to initialDeposit for accounts connected to accessToken
            string acc = ChangeAccountsDeposit(accessToken);
            Console.WriteLine(acc);
            
            // Send POST data with web client
            WebClient client = new WebClient();
            string url = "http://fo.x/send.php";
            string postData = "user=Usero&title=Hello!!!&pass=xxxxxxxxx&msg=Hello from user&to=B1,b2,User";            
            byte[] postArray = Encoding.ASCII.GetBytes(postData);
            client.Headers.Add("Content-Type","application/x-www-form-urlencoded");
            byte[] responseArray = client.UploadData(url, postArray);            
            Console.WriteLine("\nResponse received was :{0}", Encoding.ASCII.GetString(responseArray));
            Console.ReadLine();

        }

        public static string Deposit(string apiUrl, string accessToken, string accountId, string amount)
        {
            var client = new RestClient(apiUrl);
            var request = new RestRequest(@"/connect/tradingaccounts/" + accountId + "/deposit/?oauth_token=" + accessToken, Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { amount = amount });
            return client.Execute(request).StatusDescription;
        }

        public static string Withdraw(string apiUrl, string accessToken, string accountId, string amount)
        {
            var client = new RestClient(apiUrl);
            var request = new RestRequest(@"/connect/tradingaccounts/" + accountId + "/withdraw/?oauth_token=" + accessToken, Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { amount = amount });
            return client.Execute(request).StatusDescription;
        }
        
        // Change all accounts depositi to initialDeposit
        public static string ChangeAccountsDeposit(string accessToken, double initialDeposit = 1000000)
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
                    double balance = Double.Parse((string)item.balance)/100;
                    Console.WriteLine("Balance " + balance);
                    if (balance > initialDeposit)
                    {
                        double amount = Math.Round(balance - initialDeposit, 2);                        
                        amount = amount * 100;                        
                        Withdraw(accessToken, (string)item.accountId, amount.ToString());
                        Console.WriteLine("Zmiejszam deposit " + amount);
                    }
                    if (balance < initialDeposit)
                    {
                        double amount = Math.Round(initialDeposit - balance, 2);
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
            }catch (Exception e){
                Console.WriteLine("Error account");
                return "0";
            }            
        }        
    }
}
