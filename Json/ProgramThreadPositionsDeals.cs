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
        public static string accessToken = "";

        static void Main(string[] args)
        {
            // Create databases if not exists
            if (!BreakermindForex.CreateDatabase()) return;
            while (true)
            {
                // Show timestamp
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                BreakermindForex.Print("\r\nBreakermind.com All rights reserved.\n");
                BreakermindForex.Print("Start time: " + DateTime.UtcNow.ToString() + " [" + BreakermindForex.Timestamp().ToString() + "]" + "\r\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.White;

                // get open positions
                BreakermindForex.Print("\r\nNow Positions Show \r\n");                
                BreakermindForex.GetPositions(accessToken);
                                
                // Get deals
                BreakermindForex.Print("\r\nNow Deals Show \r\n");                
                BreakermindForex.GetDeals(accessToken);

                Thread.Sleep(2000);
            }
        }
    }
}
