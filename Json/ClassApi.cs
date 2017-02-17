using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ConnectApi
{
    class ClassAPi
    {
    }

    public class cTraderDeals
    {
        public List<cTraderDeal> data { get; set; }
    }

    public class cTraderDeal
    {
        public string dealId { get; set; }
        public string positionId { get; set; }
        public string orderId { get; set; }
        public string tradeSide { get; set; }
        public string volume { get; set; }
        public string filledVolume { get; set; }
        public string symbolName { get; set; }
        public string commision { get; set; }
        public string executionPrice { get; set; }
        public string baseToUsdConversionRate { get; set; }
        public string marginRate { get; set; } 
        public string chanel { get; set; } 
        public string label { get; set; }
        public string comment { get; set; }
        public string createTimestamp { get; set; }
        public string executionTimestamp { get; set; }
        public cTraderDealCloseDetails positionCloseDetails { get; set; } = null;        
    }

    public class cTraderDealCloseDetails
    {
        public string entryPrice { get; set; }
        public string profit { get; set; }
        public string swap { get; set; }
        public string commision { get; set; }
        public string balance { get; set; }
        public string balanceVersion { get; set; }
        public string comment { get; set; }
        public string stopLossPrice { get; set; }
        public string takeProfitPrice { get; set; }
        public string quoteToDepositConversionRate { get; set; }
        public string closedVolume { get; set; }
        public string profitInPips { get; set; }
        public string roi { get; set; }
        public string equityBasedRoi { get; set; }
        public string equity { get; set; }
    }

    public class cTraderPositions
    {
        public List<cTraderPosition> data { get; set; }
    }

    public class cTraderPosition
    {
        public string positionId { get; set; }
        public string entryTimestamp { get; set; }
        public string utcLastUpdateTimestamp { get; set; }
        public string symbolName { get; set; }
        public string tradeSide { get; set; }
        public string entryPrice { get; set; }
        public string volume { get; set; }
        public string stopLoss { get; set; }
        public string takeProfit { get; set; }
        public string profit { get; set; }
        public string profitInPips { get; set; }
        public string commision { get; set; }
        public string marginRate { get; set; }
        public string swap { get; set; }
        public string currentPrice { get; set; }
        public string comment { get; set; }
        public string chanel { get; set; }
        public string label { get; set; }
    }

    public class cTraderAccounts
    {
        public List<cTraderAccount> data { get; set; }
    }

    public class cTraderAccount
    {
        public string accountId  { get; set; }        
        public string accountNumber { get; set; }
        public string live { get; set; }
        public string brokerName { get; set; }
        public string brokerTitle{ get; set; }
        public string brokerCode { get; set; }
        public string depositCurrency { get; set; }
        public string traderRegistrationTimestamp { get; set; }
        public string traderAccountType { get; set; }
        public string leverage { get; set; }
        public string leverageInCents { get; set; }
        public string balance { get; set; }
        public string deleted { get; set; }
        public string accountStatus { get; set; }
        public string swapFree { get; set; }
    }



    //====================================================================================================================
    // Create database if not exist    
    //====================================================================================================================
    public class BreakermindForex
    {
        public static string connectionString;
        public static MySqlConnection c;
        public static string DBname = "BreakermindForex";
        public static string server = "localhost";
        public static string uid = "root";
        public static string password = "toor";
        public static int Port = 3306;
        public static bool SslMode = true;

        // Save errors to logs
        public static isError err = new isError();

        public BreakermindForex()
        {
        }

        public static Int32 Timestamp()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static string GetIPAddress()
        {
            IPAddress[] ipList = Dns.GetHostAddresses(Dns.GetHostName());
            return ipList[0].ToString();
            //return "0.0.0.0";
        }

        // Print text
        public static void Print(string txt)
        {
            Console.WriteLine(txt.ToString());
        }

        // create database        
        public static bool CreateDatabase()
        {
            try
            {
                string connString = "SERVER=" + server + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";Port=" + Port + ";";
                if (SslMode)
                {
                    connString = "SERVER=" + server + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";Port=" + Port + ";SslMode=Required;";
                }
                MySqlConnection conn = new MySqlConnection(connString);
                // create account database
                string query = "CREATE DATABASE IF NOT EXISTS `" + DBname + "` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                // create tables;
                string t0 = "USE " + DBname + ";";
                string t1 = "CREATE TABLE IF NOT EXISTS `accessTokens` (`id` bigint(22) NOT NULL AUTO_INCREMENT,`code` varchar(250),`accessToken` varchar(250),`refreshToken` varchar(250),`tokenType` varchar(250),`expiresIn` varchar(250),`refreshTime` bigint(22) DEFAULT 0,`ip` varchar(250),`time` bigint(22) DEFAULT 0,`active` char(1) DEFAULT '1',`uniqueToken` varchar(250),PRIMARY KEY (`id`),UNIQUE KEY `accessToken` (`accessToken`)) ENGINE=InnoDB DEFAULT CHARSET=`utf8` COLLATE=`utf8_general_ci`;";
                string t2 = "CREATE TABLE IF NOT EXISTS `accessTokensInactive` (`id` bigint(22) NOT NULL AUTO_INCREMENT,`code` text,`accessToken` text,`refreshToken` text,`tokenType` varchar(250),`expiresIn` varchar(250),`refreshTime` bigint(22) DEFAULT 0,`ip` varchar(250),`time` bigint(22) DEFAULT 0,PRIMARY KEY (`id`)) ENGINE=InnoDB DEFAULT CHARSET=`utf8` COLLATE=`utf8_general_ci`;";
                string t3 = "CREATE TABLE IF NOT EXISTS `users` (`id` bigint(22) NOT NULL AUTO_INCREMENT,`email` varchar(200),`nick` varchar(50),`pass` varchar(32),`country` varchar(100) DEFAULT NULL,`mobile` varchar(100) DEFAULT NULL,`www` varchar(250) DEFAULT NULL,`about` text,`trader` char(1) DEFAULT '0',`ip` varchar(100),`time` bigint(22) DEFAULT 0,`active` char(1) DEFAULT '1',PRIMARY KEY (`id`),UNIQUE KEY `nick` (`nick`)) ENGINE=InnoDB DEFAULT CHARSET=`utf8` COLLATE=`utf8_general_ci`;";
                string t4 = "CREATE TABLE IF NOT EXISTS `accounts` (`id` bigint NOT NULL AUTO_INCREMENT,`accountId` varchar(250),`accountNumber` varchar(250),`live` varchar(250),`brokerName` varchar(250),`brokerTitle` varchar(250),`brokerCode` varchar(250),`depositCurrency` varchar(250),`traderRegistrationTimestamp` varchar(250),`traderAccountType` varchar(250),`leverage` varchar(250),`leverageInCents` varchar(250),`balance` varchar(250),`deleted` varchar(250),`accountStatus` varchar(250),`swapFree` varchar(250),`ip` varchar(250),`time` bigint(22) DEFAULT 0,`active` char(1) DEFAULT '1',`unique` varchar(250),PRIMARY KEY (`id`),UNIQUE KEY `accountId`(`accountId`)) ENGINE=InnoDB DEFAULT CHARSET=`utf8` COLLATE=`utf8_general_ci`;";
                string t5 = "CREATE TABLE IF NOT EXISTS `OpenPositions` (`id` bigint NOT NULL AUTO_INCREMENT,`accountId` varchar(250),`accountNumber` varchar(250),`positionId` varchar(250),`entryTimestamp` varchar(250),`utcLastUpdateTimestamp` varchar(250),`symbolName` varchar(50),`tradeSide` varchar(50),`entryPrice` decimal(10,6),`volume` decimal(50,2),`stopLoss` decimal(10,2),`takeProfit` decimal(10,2),`profit` decimal(10,2),`profitInPips` decimal(10,2),`commision` decimal(10,2),`marginRate` decimal(10,2),`swap` decimal(10,2),`currentPrice` decimal(10,6),`comment` varchar(250),`chanel` varchar(250),`label` varchar(250),`ip` varchar(250),`time` bigint(22) DEFAULT 0,`active` char(1) DEFAULT '1',`unique` varchar(250),PRIMARY KEY (`id`),UNIQUE KEY `positionId`(`positionId`),UNIQUE KEY `unique` (`accountNumber`,`positionId`)) ENGINE=InnoDB DEFAULT CHARSET=`utf8` COLLATE=`utf8_general_ci`;";
                string t6 = "CREATE TABLE IF NOT EXISTS `ClosePositions` (`id` bigint NOT NULL AUTO_INCREMENT,`accountId` varchar(250),`accountNumber` varchar(250),`dealId` varchar(250),`positionId` varchar(250),`orderId` varchar(250),`tradeSide` varchar(250),`volume` decimal(50,2),`filledVolume` decimal(50,2),`symbolName` varchar(250),`commision` decimal(50,2),`executionPrice` decimal(50,6),`baseToUsdConversionRate` varchar(250),`marginRate` varchar(250),`chanel` varchar(250),`label` varchar(250),`comment` varchar(250),`createTimestamp` varchar(250),`executionTimestamp` varchar(250),`positionCloseDetails` char(1), `entryPrice` decimal(50,6),`profit` decimal(50,2),`swap` decimal(50,2),`commisionclose` decimal(50,2),`balance` decimal(50,2),`balanceVersion` varchar(50),`commentclose` varchar(250),`stopLossPrice` decimal(50,2),`takeProfitPrice` decimal(50,2),`quoteToDepositConversionRate` decimal(50,6),`closedVolume` decimal(50,2),`profitInPips` decimal(50,2),`roi` decimal(50,6),`equityBasedRoi` decimal(50,6),`equity` decimal(50,6),`ip` varchar(250),`time` bigint(22) DEFAULT 0,`active` char(1) DEFAULT '1',`unique` varchar(250),PRIMARY KEY (`id`),UNIQUE KEY `positionId`(`positionId`),UNIQUE KEY `unique` (`accountNumber`,`positionId`)) ENGINE=InnoDB DEFAULT CHARSET=`utf8` COLLATE=`utf8_general_ci`;";                

                cmd = new MySqlCommand(t0 + t1 + t2 + t3 + t4 + t5 + t6, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                Print("Database " + DBname + " has been created.");
                return true;
            }
            catch (Exception e)
            {
                Print("Create database:  " + e.Message);
                //Print("Hit key ...");
                //Console.ReadKey();
                return false;
            }
        }

        // add position to database
        public static void PositionAdd(string accountId, string accountNumber, string positionId, string entryTimestamp, string utcLastUpdateTimestamp, string symbolName, string tradeSide, string entryPrice, string volume, string stopLoss, string takeProfit, string profit, string profitInPips, string commision, string marginRate, string swap, string currentPrice, string comment, string chanel, string label, string ip, string time)
        {
            // Console.WriteLine(connectionString.ToString());
            if (commision == "" || commision == null)
            {
                commision = "0";
            }
            if (chanel == "" || chanel == null)
            {
                chanel = "";
            }
            if (comment == "" || comment == null)
            {
                comment = "";
            }
            if (takeProfit == "" || takeProfit == null)
            {
                takeProfit = "0";
            }
            if (stopLoss == "" || stopLoss == null)
            {
                stopLoss = "0";
            }
            string query = "INSERT INTO OpenPositions (accountId,accountNumber,positionId,entryTimestamp,utcLastUpdateTimestamp,symbolName,tradeSide,entryPrice,volume,stopLoss,takeProfit,profit,profitInPips,commision,marginRate,swap,currentPrice,comment,chanel,label,ip,time) " +
                "VALUES('" + accountId + "','" + accountNumber + "', '" + positionId + "','" + entryTimestamp + "','" + utcLastUpdateTimestamp + "','" + symbolName + "','" + tradeSide + "','" + Double.Parse(entryPrice) + "','" + Double.Parse(volume) + "','" + Double.Parse(stopLoss) + "','" + Double.Parse(takeProfit) + "','" + Double.Parse(profit) + "','" + Double.Parse(profitInPips) + "','" + Double.Parse(commision) + "','" + Double.Parse(marginRate) + "','" + Double.Parse(swap) + "','" + Double.Parse(currentPrice) + "','" + comment + "','" + chanel + "','" + label + "','" + ip + "','" + time + "') " +
                "ON DUPLICATE KEY UPDATE  stopLoss='" + stopLoss + "',  takeProfit='" + takeProfit + "', profit='" + Double.Parse(profit) + "', profitInPips='" + Double.Parse(profitInPips) + "'";

            connectionString = "SERVER=" + server + ";" + "DATABASE=" + DBname + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";Port=" + Port + ";";
            if (SslMode)
            {
                connectionString = "SERVER=" + server + ";" + "DATABASE=" + DBname + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";Port=" + Port + ";SslMode=Required;";
            }

            try
            {
                MySqlConnection c1 = new MySqlConnection(connectionString);
                c1.Open();
                MySqlCommand cmd = new MySqlCommand(query, c1);
                cmd.ExecuteNonQuery();
                Print("Add position " + positionId);
                c1.Close();
            }
            catch (Exception e)
            {
                Print("Error update " + positionId + " " + e);
            }
        }

        // Get open positions
        public static void GetPositions (string accessToken)
        {
            string urlAccounts = "https://api.spotware.com/connect/tradingaccounts?access_token=" + accessToken;
            string cAccounts = "";
            // string urlData = @"{'data':[{'positionId':7980418,'entryTimestamp':1487151223210,'utcLastUpdateTimestamp':1487151223210,'symbolName':'GBPUSD','tradeSide':'BUY','entryPrice':1.2428,'volume':100000,'stopLoss':null,'takeProfit':null,'profit':-338,'profitInPips':-35.7,'commission':-3,'marginRate':1.17765,'swap':0,'currentPrice':1.23923,'comment':null,'channel':'cAlgo','label':null}]}";            

            try
            {
                try
                {
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
                            // Console.WriteLine(txt + "\nPositions from account " + acc.accountId + " >>> " + cAccountPositions);
                            Console.WriteLine(txt + "\nPositions from account " + acc.accountId + " >>> \r\n");

                            // Deserialize to object class cTraderPositions
                            cTraderPositions positions = JsonConvert.DeserializeObject<cTraderPositions>(cAccountPositions);
                            foreach (cTraderPosition p in positions.data)
                            {
                                // Add position to database
                                Int32 Timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                                BreakermindForex.PositionAdd(acc.accountId, acc.accountNumber, p.positionId, p.entryTimestamp, p.utcLastUpdateTimestamp, p.symbolName, p.tradeSide, p.entryPrice, p.volume, p.stopLoss, p.takeProfit, p.profit, p.profitInPips, p.commision, p.marginRate, p.swap, p.currentPrice, p.comment, p.chanel, p.label, BreakermindForex.GetIPAddress(), Timestamp.ToString());
                            }
      
                            // log to file
                            err.SavePositions(DateTime.UtcNow.ToString() + "  " + txt + "\nPositions from account " + acc.accountId + " >>> " + cAccountPositions);
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(DateTime.UtcNow.ToString() + "Coś nie tak z pobieraniem pozycji \r\n" + ee);
                            // log to file
                            err.SaveError(ee.ToString());
                        }
                    });
                    thread.Start();
                    Thread.Sleep(2002);
                }
             }
            catch (Exception e)
            {
                Console.WriteLine("Spotwer error : " + e.ToString());
                // log to file
                err.SaveError(e.ToString());
            }
        }

        // add deal to database
        public static void DealAdd(string accountId, string accountNumber, string dealId, string positionId, string orderId, string tradeSide, string volume, string filledVolume, string symbolName, string commision, string executionPrice, string baseToUsdConversionRate, string marginRate, string chanel, string label, string comment, string createTimestamp, string executionTimestamp, string positionCloseDetails, string entryPrice, string profit, string swap, string commisionclose, string balance, string balanceVersion, string commentclose, string stopLossPrice, string takeProfitPrice, string quoteToDepositConversionRate, string closedVolume, string profitInPips, string roi, string equityBasedRoi, string equity, string ip, string time)
        {
            
            if (profit == "" || profit == null) profit = "0";
            if (marginRate == "" || marginRate == null) marginRate = "0";
            if (positionCloseDetails == "" || positionCloseDetails == null) positionCloseDetails = "0";
            if (quoteToDepositConversionRate == "" || quoteToDepositConversionRate == null) quoteToDepositConversionRate = "0";
            if (executionPrice == "" || executionPrice == null) executionPrice = "0";            
            if (balance == "" || balance == null)balance = "0";            
            if (commision == "" || commision == null) commision = "0";            
            if (chanel == "" || chanel == null) chanel = "0";
            if (comment == "" || comment == null) comment = "0";
            if (takeProfitPrice == "" || takeProfitPrice == null) takeProfitPrice = "0";
            if (stopLossPrice == "" || stopLossPrice == null) stopLossPrice = "0";
            if (swap == "" || swap == null)  swap = "0";
            if (commisionclose == "" || commisionclose == null)  commisionclose = "0";
            if ( closedVolume== "" ||  closedVolume== null)  closedVolume= "0";
            if ( profitInPips== "" ||  profitInPips== null) profitInPips = "0";
            if ( roi== "" || roi == null) roi = "0";
            if ( equityBasedRoi== "" ||  equityBasedRoi== null)  equityBasedRoi= "0";
            if ( equity== "" ||  equity== null)  equity= "0";
            if ( label == "" || label == null) label = "0";
            if ( commentclose == "" || commentclose == null) commentclose = "0";

            string query = "INSERT INTO ClosePositions (accountId,accountNumber,dealId,positionId,orderId,tradeSide,volume,filledVolume,symbolName,commision,executionPrice,baseToUsdConversionRate,marginRate,chanel,label,comment,createTimestamp,executionTimestamp,positionCloseDetails,entryPrice,profit,swap,commisionclose,balance,balanceVersion,commentclose,stopLossPrice,takeProfitPrice,quoteToDepositConversionRate,closedVolume,profitInPips,roi,equityBasedRoi,equity,ip,time) " +
                "VALUES('" + accountId + "','" + accountNumber + "','" + dealId + "','" + positionId + "','" + orderId + "','" + tradeSide + "','" + volume + "','" + filledVolume + "','" + symbolName + "','" + Double.Parse(commision) + "','" + Convert.ToDouble(executionPrice) + "','" + baseToUsdConversionRate + "','" + Convert.ToDouble(marginRate) + "','" + chanel + "','" + label + "','" + comment + "','" + createTimestamp + "','" + executionTimestamp + "','" + positionCloseDetails + "','" + Convert.ToDouble(entryPrice) + "','" + Double.Parse(profit) + "','" + Double.Parse(swap) + "','" + Double.Parse(commisionclose) + "','" + Double.Parse(balance) + "','" + balanceVersion + "','" + commentclose + "','" + Double.Parse(stopLossPrice) + "','" + Double.Parse(takeProfitPrice) + "','" + Double.Parse(quoteToDepositConversionRate) + "','" + Double.Parse(closedVolume) + "','" + Double.Parse(profitInPips) + "','" + Double.Parse(roi) + "','" + Double.Parse(equityBasedRoi, CultureInfo.InvariantCulture) + "','" + Double.Parse(equity) + "','" + ip + "','" + time + "') " +
                "ON DUPLICATE KEY UPDATE executionPrice='"+ Convert.ToDouble(executionPrice) + "', stopLossPrice='" + Double.Parse(stopLossPrice) + "',  takeProfitPrice='" + Double.Parse(takeProfitPrice) + "', profit='" + Double.Parse(profit) + "', profitInPips='" + Double.Parse(profitInPips) + "'";

            connectionString = "SERVER=" + server + ";" + "DATABASE=" + DBname + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";Port=" + Port + ";";
            if (SslMode)
            {
                connectionString = "SERVER=" + server + ";" + "DATABASE=" + DBname + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";Port=" + Port + ";SslMode=Required;";
            }

            try
            {
                MySqlConnection c1 = new MySqlConnection(connectionString);
                c1.Open();
                MySqlCommand cmd = new MySqlCommand(query, c1);
                cmd.ExecuteNonQuery();
                Print("Add deal " + positionId);
                c1.Close();
            }
            catch (Exception e)
            {
                Print("Error update deal " + positionId + " " + e);
            }
        }

        // Get deals positions 
        public static void GetDeals(string accessToken)
        {
            // string urlData = @"{'data':[{'positionId':7980418,'entryTimestamp':1487151223210,'utcLastUpdateTimestamp':1487151223210,'symbolName':'GBPUSD','tradeSide':'BUY','entryPrice':1.2428,'volume':100000,'stopLoss':null,'takeProfit':null,'profit':-338,'profitInPips':-35.7,'commission':-3,'marginRate':1.17765,'swap':0,'currentPrice':1.23923,'comment':null,'channel':'cAlgo','label':null}]}";
            string urlAccounts = "https://api.spotware.com/connect/tradingaccounts?access_token=" + accessToken;
            string cAccounts = "";            

            try
            {
                try
                {
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

                // Deserialize to object class cTraderAccounts
                cTraderAccounts result = JsonConvert.DeserializeObject<cTraderAccounts>(cAccounts);
                // var aId = result.data[0].accountId;

                // show all accounts Use:  result.data !!!!!!!!
                foreach (cTraderAccount acc in result.data)
                {
                    Thread thread = new Thread(() =>
                    {
                        try
                        {
                            string txt = "\r\n" + "Account " + acc.accountId + " Broker " + acc.brokerName + " Account Number " + acc.accountNumber;
                            WebClient pos = new WebClient();
                            string cAccountDeals = pos.DownloadString("https://api.spotware.com/connect/tradingaccounts/" + acc.accountId + "/deals?access_token=" + accessToken);
                            // Console.WriteLine(txt + "\nDeals from account " + acc.accountId + " >>> " + cAccountDeals);
                            
                            Console.WriteLine(txt + "\nDeals from account " + acc.accountId + " >>> \r\n");

                            // Deserialize to object class cTraderPositions
                            cTraderDeals deals = JsonConvert.DeserializeObject<cTraderDeals>(cAccountDeals);
                            foreach (cTraderDeal p in deals.data)
                            {
                                // Add position to database
                                Int32 Timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                                if (p.positionCloseDetails != null)
                                {
                                    /*
                                    if (p.orderId == null) p.orderId = "0";
                                    if (p.tradeSide == null)p.tradeSide  = "0";
                                    if (p.volume == null)  p.volume = "0";
                                    if (p.filledVolume == null)p.filledVolume  = "0";
                                    if (p.symbolName == null)p.symbolName  = "0";
                                    if (p.commision == null)p.commision  = "0";
                                    if (p.executionPrice == null)p.executionPrice  = "0";
                                    if (p.baseToUsdConversionRate == null)p.baseToUsdConversionRate  = "0";
                                    if (p.marginRate == null)p.marginRate  = "0";
                                    if (p.chanel == null)p.chanel  = "0";
                                    if (p.label == null)p.label  = "0";
                                    if (p.comment == null)p.comment  = "0";
                                    if (p.createTimestamp == null)p.createTimestamp  = "0";
                                    if (p.executionTimestamp == null)p.executionTimestamp  = "0";
                                    // Details
                                    if (p.positionCloseDetails.entryPrice == null)p.positionCloseDetails.entryPrice  = "0";
                                    if (p.positionCloseDetails.profit == null) p.positionCloseDetails.profit  = "0";
                                    if (p.positionCloseDetails.swap == null) p.positionCloseDetails.swap  = "0";
                                    if (p.positionCloseDetails.commision == null) p.positionCloseDetails.commision  = "0";
                                    if (p.positionCloseDetails.balance == null) p.positionCloseDetails.balance  = "0";
                                    if (p.positionCloseDetails.balanceVersion == null) p.positionCloseDetails.balanceVersion  = "0";
                                    if (p.positionCloseDetails.comment == null) p.positionCloseDetails.comment  = "0";
                                    if (p.positionCloseDetails.stopLossPrice == null) p.positionCloseDetails.stopLossPrice  = "0";
                                    if (p.positionCloseDetails.takeProfitPrice == null) p.positionCloseDetails.takeProfitPrice  = "0";
                                    if (p.positionCloseDetails.quoteToDepositConversionRate == null) p.positionCloseDetails.quoteToDepositConversionRate  = "0";
                                    if (p.positionCloseDetails.closedVolume == null) p.positionCloseDetails.closedVolume  = "0";
                                    if (p.positionCloseDetails.profitInPips == null) p.positionCloseDetails.profitInPips  = "0";
                                    if (p.positionCloseDetails.roi == null) p.positionCloseDetails.roi  = "0";
                                    if (p.positionCloseDetails.equityBasedRoi == null) p.positionCloseDetails.equityBasedRoi  = "0";
                                    if (p.positionCloseDetails.equity == null) p.positionCloseDetails.equity  = "0";
                                    */

                                    BreakermindForex.DealAdd(acc.accountId, acc.accountNumber, p.dealId, p.positionId, p.orderId, p.tradeSide, p.volume, p.filledVolume, p.symbolName, p.commision, p.executionPrice, p.baseToUsdConversionRate, p.marginRate, p.chanel, p.label, p.comment, p.createTimestamp, p.executionTimestamp, "1", p.positionCloseDetails.entryPrice, p.positionCloseDetails.profit, p.positionCloseDetails.swap, p.positionCloseDetails.commision, p.positionCloseDetails.balance, p.positionCloseDetails.balanceVersion, p.positionCloseDetails.comment, p.positionCloseDetails.stopLossPrice, p.positionCloseDetails.takeProfitPrice, p.positionCloseDetails.quoteToDepositConversionRate, p.positionCloseDetails.closedVolume, p.positionCloseDetails.profitInPips, p.positionCloseDetails.roi, p.positionCloseDetails.equityBasedRoi, p.positionCloseDetails.equity, BreakermindForex.GetIPAddress().ToString(), Timestamp.ToString());
                                }else
                                {
                                   BreakermindForex.DealAdd(acc.accountId, acc.accountNumber, p.dealId, p.positionId, p.orderId, p.tradeSide, p.volume, p.filledVolume, p.symbolName, p.commision, p.executionPrice, p.baseToUsdConversionRate, p.marginRate, p.chanel, p.label, p.comment, p.createTimestamp, p.executionTimestamp, "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", BreakermindForex.GetIPAddress(), Timestamp.ToString());
                                }
                            }

                            // log to file
                            err.SavePositions(DateTime.UtcNow.ToString() + "  " + txt + "\nDeals from account " + acc.accountId + " >>> " + cAccountDeals);
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(DateTime.UtcNow.ToString() + "Coś nie tak z pobieraniem pozycji \r\n" + ee);
                            // log to file
                            err.SaveError(ee.ToString());
                        }
                    });
                    thread.Start();
                    Thread.Sleep(2002);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Spotware error : " + e.ToString());
                // log to file
                err.SaveError(e.ToString());
            }
        }

    } // Class


    public class isError
    {
        public bool SaveError(string txt)
        {
            try { 
                // Save to file
                if (txt != null)
                {
                    string dir = Directory.GetCurrentDirectory();
                    System.IO.StreamWriter file = new System.IO.StreamWriter(dir + @"\error.txt", true);
                    file.WriteLine(DateTime.UtcNow.ToString() + " || " + txt);
                    file.Close();
                    return true;
                }
            }catch (Exception rr)
            {
                Console.WriteLine(rr.ToString());
            }
            return false;
        }

        public bool SavePositions(string txt)
        {
            try
            {
                // Save to file
                if (txt != null)
                {
                    string dir = Directory.GetCurrentDirectory();
                    System.IO.StreamWriter file = new System.IO.StreamWriter(dir + @"\positions.txt", true);
                    file.WriteLine(DateTime.UtcNow.ToString() + " || " + txt);
                    file.Close();
                    return true;
                }
            }
            catch (Exception rr)
            {
                Console.WriteLine(rr.ToString());
            }
            return false;
        }

    }
}
