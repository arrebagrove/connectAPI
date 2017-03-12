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
    }
}
