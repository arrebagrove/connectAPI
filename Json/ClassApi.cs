using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConnectApi
{
    class ClassAPi
    {
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
        //public static bool CreateDatabase(string DBname = "BreakermindForex", string server = "localhost", string uid = "root", string password = "toor", int Port = 3306, bool SslMode = true)
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
                string t6 = "CREATE TABLE IF NOT EXISTS `ClosePositions` (`id` bigint NOT NULL AUTO_INCREMENT,`accountId` varchar(250),`accountNumber` varchar(250),`positionId` varchar(250),`executionPrice` decimal(10,6),`entryPrice` decimal(10,6),`profit` decimal(10,2),`swap` decimal(10,2),`commision` decimal(10,2),`balance` decimal(10,2),`balanceVersion` varchar(50),`comment` varchar(250),`stopLossPrice` varchar(50),`takeProfitPrice` varchar(50),`quoteToDepositConversionRate` decimal(10,6),`closedVolume` decimal(50,2),`profitInPips` decimal(10,2),`roi` decimal(50,6),`equityBasedRoi` decimal(50,6),`equity` decimal(50,6),`ip` varchar(250),`time` bigint(22) DEFAULT 0,`active` char(1) DEFAULT '1',`unique` varchar(250),PRIMARY KEY (`id`),UNIQUE KEY `positionId`(`positionId`),UNIQUE KEY `unique` (`accountNumber`,`positionId`)) ENGINE=InnoDB DEFAULT CHARSET=`utf8` COLLATE=`utf8_general_ci`;";

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
                "VALUES('" + accountId + "','" + accountNumber + "', '" + positionId + "','" + entryTimestamp + "','" + utcLastUpdateTimestamp + "','" + symbolName + "','" + tradeSide + "','" + decimal.Parse(entryPrice) + "','" + decimal.Parse(volume) + "','" + decimal.Parse(stopLoss) + "','" + decimal.Parse(takeProfit) + "','" + decimal.Parse(profit) + "','" + decimal.Parse(profitInPips) + "','" + decimal.Parse(commision) + "','" + decimal.Parse(marginRate) + "','" + decimal.Parse(swap) + "','" + decimal.Parse(currentPrice) + "','" + comment + "','" + chanel + "','" + label + "','" + ip + "','" + time +"') "+
                "ON DUPLICATE KEY UPDATE  stopLoss='" + stopLoss + "',  takeProfit='" + takeProfit + "', profit='" + decimal.Parse(profit) + "', profitInPips='" + decimal.Parse(profitInPips) + "'";
           
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
            catch(Exception e)
            {
                Print("Error update " + positionId + " " + e);
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
