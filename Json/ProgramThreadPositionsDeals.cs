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
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Green;
                BreakermindForex.Print("\r\n Now Positions Show");
                // get open positions
                BreakermindForex.GetPositions(accessToken);
                Console.ResetColor();

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                BreakermindForex.Print("\r\n Now Deals Show \r\n");
                // Get deals
                BreakermindForex.GetDeals(accessToken);
                Console.ResetColor();
            }
        }
    }
}