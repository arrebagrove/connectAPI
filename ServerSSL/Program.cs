using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.Collections;

//Add MySql Library
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Cryptography;
using NetFwTypeLib;
using System.Linq;

namespace server
{
    public class server
    {
        // Server IP and Port
        public static string HostIP = "0.0.0.0";
        public static int Port = 1234;
        
        // Show last 50 closed positions (false) or from last hour (true)
        public static bool ClosePositionsShowLastHour = true;
        
        // Mysql Connection variables             
        public static string mhost = "localhost";
        public static string mdatabase = "green";
        public static string muser = "root";
        public static string mpass = "toor";
        public static string con = "SERVER=" + mhost + ";" + "DATABASE=" + mdatabase + ";" + "UID=" + muser + ";" + "PASSWORD=" + mpass+ ";";

        // SSL Certyficate connect
        X509Certificate serverCertificate = new X509Certificate2("ssl1.pfx", "password12345");

        // public static ArrayList arrSocket = new ArrayList();
        // private string[,] posArray = new string[maxSlots + 1, 1];

        // Reset client
        ManualResetEvent tcpClientConnected = new ManualResetEvent(false);

        //is user can copy positions return > 0 if can't copy 0
        public static Int64 allowCopy(string user, string pass, Int64 copyid)
        {
            //Int64 trid = getUserID(user,pass,copyid);
            Int64 id = 0;
            try
            {
                // Secure Mysql
                //string query = "SELECT id FROM user WHERE user=@User AND pass=@Pass LIMIT 1";
                //MySqlCommand cmd = new MySqlCommand(query, connection);
                //cmd.Parameters.AddWithValue("@User", user);
                //cmd.Parameters.AddWithValue("@Pass", user);
                // or
                // cmd.SelectCommand.Parameters.Add("@au_id", SqlDbType.VarChar, 11);
                // cmd.SelectCommand.Parameters["@au_id"].Value = BooID

                MySqlConnection connection = new MySqlConnection(con);
                connection.Open();
                //string query = "SELECT id FROM accounts_copy WHERE nr=" + copyid + " AND trid=" + trid + " AND month=" + DateTime.UtcNow.Month + " AND year=" + DateTime.UtcNow.Year +" LIMIT 1;";
                //string query = "SELECT id FROM accounts_copy WHERE nr=@Copyid AND trid=@Trid AND month=" + DateTime.UtcNow.Month + " AND year=" + DateTime.UtcNow.Year + " LIMIT 1;";
                string query = "SELECT id FROM accounts_copy WHERE nr=@Copyid AND trnick=@Trnick AND month=" + DateTime.UtcNow.Month + " AND year=" + DateTime.UtcNow.Year + " AND status='1' AND active='1' LIMIT 1;";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Copyid", copyid);
                cmd.Parameters.AddWithValue("@Trnick", user);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {                    
                    id = Int64.Parse(dataReader["id"].ToString());                    
                }                
                dataReader.Close();
                connection.Close();
                return id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }

        public static Int64 getUserID(string user, string pass, Int64 copyid)
        {
            Int64 trid = 0;
            string passhash = md5(pass);
            try
            {
                MySqlConnection connection = new MySqlConnection(con);
                connection.Open();
                string query = "SELECT * FROM users WHERE nick=@User AND pass='" + passhash + "';";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@User", user);                
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    trid =  Int64.Parse(dataReader["id"].ToString());                    
                }
                dataReader.Close();
                connection.Close();                
                return trid;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }

        public static string md5(string pass)
        {
            // given, a password in a string
            string password = pass;
            // byte array representation of that string
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);
            // need MD5 to calculate the hash
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            // string representation (similar to UNIX format)
            string encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            return encoded;
        }

        // Validate input if needed
        public void CreateNewUserAccount(string name, string password)
        {
            // Check name contains only lower case or upper case letters, the apostrophe, a dot, or white space. Also check it is between 1 and 40 characters long
            if (!Regex.IsMatch(name, @"^[a-zA-Z'./s]{1,50}$"))
                throw new FormatException("Invalid name format");

            // Check password contains at least one digit, one lower case letter, one uppercase letter, and is between 8 and 10 characters long
            if (!Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,50}$"))
                throw new FormatException("Invalid password format");            
         }

        //Select statement
        public static string SelectOpen(string user, string pass, Int64 copyid)
        {            
            try
            {
                if (allowCopy(user, pass, copyid) > 0)
                {
                    // Open positions class list (Open and close positons)
                    OpenOrders positions = new OpenOrders();

                    MySqlConnection connection = new MySqlConnection(con);
                    connection.Open();
                    string query = "SELECT accountId,positionId,entryTimestamp,symbolName,tradeSide,volume,time FROM openpositions WHERE accountId=" + copyid +
                        " AND positionId NOT IN(SELECT positionId FROM closepositions WHERE positionCloseDetails = '1') ORDER BY id DESC LIMIT 50";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        // Create position
                        OrderOpen position = new OrderOpen();
                        position.accountId = dataReader["accountid"].ToString();
                        position.positionId = dataReader["positionId"].ToString();
                        position.symbolName = dataReader["symbolName"].ToString();
                        position.tradeSide = dataReader["tradeSide"].ToString();
                        position.volume = dataReader["volume"].ToString();
                        position.entryTimestamp = dataReader["entryTimestamp"].ToString();
                        position.timestamp = dataReader["time"].ToString();
                        // Add position to Open positions list
                        positions.OpenPositions.Add(position);
                    }
                    //close Data Reader and connection
                    dataReader.Close();
                    connection.Close();

                    // get closed positions last 50 or from last hour
                    connection.Open();
                    string query2 = "SELECT accountId,positionId,executionPrice,symbolName,tradeSide,volume,executionTimestamp,time FROM closepositions WHERE accountId='" + copyid + "' AND positionCloseDetails = '1' ORDER BY id DESC LIMIT 50;";
                    if (ClosePositionsShowLastHour)
                    {
                        Int64 tm = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0))).TotalSeconds - (60*60*6); // 6H          
                        //query2 = "SELECT accountId,positionId,executionPrice,symbolName,tradeSide,volume,executionTimestamp,time FROM closepositions WHERE accountId='" + copyid + "' AND positionCloseDetails = '1' AND time = NOW() - INTERVAL 1 HOUR ORDER BY id DESC;";
                        query2 = "SELECT accountId,positionId,executionPrice,symbolName,tradeSide,volume,executionTimestamp,time FROM closepositions WHERE accountId='" + copyid + "' AND positionCloseDetails = '1' AND time > "+tm+" ORDER BY id DESC;";
                    }
                                        
                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                    MySqlDataReader dataReader2 = cmd2.ExecuteReader();

                    while (dataReader2.Read())
                    {
                        // Create position
                        OrderClose position = new OrderClose();
                        position.accountId = dataReader2["accountId"].ToString();
                        position.positionId = dataReader2["positionId"].ToString();
                        position.symbolName = dataReader2["symbolName"].ToString();
                        position.tradeSide = dataReader2["tradeSide"].ToString();
                        position.volume = dataReader2["volume"].ToString();
                        position.timestamp = dataReader2["executionTimestamp"].ToString();
                        // Add position to Open positions list
                        positions.ClosePositions.Add(position);
                    }
                    //close Data Reader and connection
                    dataReader2.Close();
                    connection.Close();

                    string out1 = "";
                    // Serialize class with positions
                    out1 = JsonConvert.SerializeObject(positions);
                    //Console.WriteLine("OpenOrders " + out1);
                    return out1;
                }else
                {
                    return "[COPY_NOT_ALLOWED_GET_ACCESS_FIRST]";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "[ERROR_INPUT_ACCOUNT]";
            }
        }

        static string ReadMessageTrader(string msg)
        {
            string pos = "";
            try
            {
                // Get open positions from accountId
                msg = msg.Replace("<||>", "");
                MsgGet userMsg = JsonConvert.DeserializeObject<MsgGet>(msg);               

                if (userMsg.Cmd == "GETOPEN")
                {
                    pos = SelectOpen(userMsg.User, userMsg.Pass, userMsg.accountId);
                }
                if (userMsg.Cmd == "GETCLOSE")
                {
                    pos = SelectOpen(userMsg.User, userMsg.Pass, userMsg.accountId);
                }
                Console.WriteLine(userMsg.User + "\r\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "[ERROR_INPUT_JSON]";              
            }

            return pos;
        }

        static string ReadMessage(SslStream sslStream)
        {
            // Read the  message sent by the client. 
            // The client signals the end of the message using the 
            // "<||>" marker.
            byte[] buffer = new byte[2048];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;
            do
            {
                // Read the client's test message.
                bytes = sslStream.Read(buffer, 0, buffer.Length);
                
                // Use Decoder class to convert from bytes to UTF8 
                // in case a character spans two buffers.
                Decoder decoder = Encoding.UTF8.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);
                // Check for || or an empty message. 
                if (messageData.ToString().IndexOf("<||>") != -1)
                {
                    break;
                }
            } while (bytes != 0);
            //Console.WriteLine("FROM READMESSAGE " + messageData);

            return ReadMessageTrader(messageData.ToString());
            //return messageData.ToString();
        }


        void ProcessIncomingData(object obj)
        {
            SslStream sslStream = (SslStream)obj;            
            try
            {
                //
                // Set timeouts for the read and write to 5 seconds.
                sslStream.ReadTimeout = 1000;
                sslStream.WriteTimeout = 1000;
                // Read a message from the client.   
                Console.WriteLine("Waiting for client message...");
                string messageData = ReadMessage(sslStream);                
                //Console.WriteLine("Received: " + messageData);                
                // Write a message to the client.                 
                byte[] message = Encoding.UTF8.GetBytes("[MSG]" + messageData + "<||>");
                sslStream.Write(message);
                sslStream.Flush();
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        void ProcessIncomingConnection(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(ar);
            SslStream sslStream = new SslStream(client.GetStream(), false);
            //Console.WriteLine("SOCKET TYPE " + client.Connected);

            try
            {
                sslStream.AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls, true);
                sslStream.ReadTimeout = 1000;
                sslStream.WriteTimeout = 1000;

                // Show certs info
                //DisplayCertificateInformation(sslStream);                

                // remote ip address
                //Console.Write(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                IPEndPoint newclient = (IPEndPoint)client.Client.RemoteEndPoint;                
                Console.WriteLine("Connected with {0} at port {1} and Serialize {2} HASH### {3}", newclient.Address, newclient.Port, newclient.Serialize().ToString(), newclient.GetHashCode());
            }
            catch (Exception ee)
            {
                Console.WriteLine("Client without sslSocket " + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                sslStream.Close();
                client.Client.Close();
            }
            ThreadPool.QueueUserWorkItem(ProcessIncomingData, sslStream);
            tcpClientConnected.Set();            
        }

        bool BanIdiots(TcpClient client, SslStream sslStream)
        {
            string UserIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
            List<string> AllowedIP = new List<string>();
            AllowedIP.Add("92.222.77.98");
            AllowedIP.Add("127.0.0.1");

            Console.WriteLine("BanIdiots test IP " + UserIP);
            if (!AllowedIP.Contains(UserIP))
            {
                byte[] message = Encoding.UTF8.GetBytes("[MSG]Incorrect Ip Address<||>");
                //sslStream.Write(message);
                //sslStream.Flush();                
                Console.WriteLine("Incorrect IP Address " + UserIP);
                return true;
            }
            return false;
        }

        // If BanIp = true then method add Block rule for ip address 
        public static void AddFirewallRule(string ipaddress = "255.255.255.255", string RuleName = "BreakermindCom", bool BanIP = false)
        {
            string IP = ipaddress;
            string IPName = RuleName + "_Allow_" + IP;
            if (BanIP)
            {
                IPName = RuleName + "_Ban_" + IP;
            }

            try
            {
                Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
                INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
                var currentProfiles = fwPolicy2.CurrentProfileTypes;

                // Let's create a new rule
                INetFwRule2 inboundRule = (INetFwRule2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                inboundRule.Enabled = true;
                //Allow through firewall
                inboundRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                if (BanIP)
                {
                    inboundRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                }
                //Using protocol TCP
                inboundRule.Protocol = 6;
                //Port 5555
                inboundRule.LocalPorts = "5555";
                //Name of rule
                inboundRule.Name = IPName;
                // profil
                inboundRule.Profiles = currentProfiles;
                // ip
                inboundRule.RemoteAddresses = IP;

                // Now add the rule
                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                firewallPolicy.Rules.Add(inboundRule);
                //firewallPolicy.Rules.Remove(IPName);
                
                if (BanIP)
                {
                    Console.WriteLine(IP + " Firewall Update Ban IP ... " + DateTime.UtcNow);
                }else
                {
                    Console.WriteLine(IP + " Firewall Update Allowed IP ... " + DateTime.UtcNow);
                }
            }
            catch (Exception r)
            {
                Console.WriteLine("Firewall error " + r);
            }
        }

        // if removeBan set to true method remove Ban rules
        public static void RemoveFirewallRule(string RuleName = "BreakermindCom", bool removeBan = false)
        {
            if (removeBan)
            {
                RuleName = RuleName + "_Ban_";
            }else
            {
                RuleName = RuleName + "_Allow_";
            }

            try
            {
                Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
                INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
                var currentProfiles = fwPolicy2.CurrentProfileTypes;               

                // Lista rules
                // List<INetFwRule> RuleList = new List<INetFwRule>();

                foreach (INetFwRule rule in fwPolicy2.Rules)
                {
                    // Add rule to list
                    //RuleList.Add(rule);
                    // Console.WriteLine(rule.Name);
                    if (rule.Name.IndexOf(RuleName) != -1)
                    {
                        // Now add the rule
                        INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));                     
                        firewallPolicy.Rules.Remove(rule.Name);
                        Console.WriteLine(rule.Name + " has been deleted from Firewall Policy");
                    }
                }
            }
            catch (Exception r)
            {
                Console.WriteLine("Error delete rule from firewall " + r);
            }
        }

        // validate ipv4 address
        public static bool IsIPv4(string ipAddress)
        {
            return Regex.IsMatch(ipAddress, @"^\d{1,3}(\.\d{1,3}){3}$") && ipAddress.Split('.').SingleOrDefault(s => int.Parse(s) > 255) == null;
        }

        // Update list with allowed ip addresses
        public static void UpdateAllowedIP()
        {
            List<string> AllowedIPAddress = new List<string>();
            MySqlConnection connection = new MySqlConnection(con);
            connection.Open();
            string query = "SELECT copyfromip FROM users WHERE active ='1';";
            MySqlCommand cmd = new MySqlCommand(query, connection);            
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                dataReader["copyfromip"].ToString().Replace("/","");
                if (dataReader["copyfromip"].ToString() != null && dataReader["copyfromip"].ToString() != "" && dataReader["copyfromip"].ToString() != "0.0.0.0")
                {
                    if (IsIPv4(dataReader["copyfromip"].ToString()))
                    {
                        AllowedIPAddress.Add(dataReader["copyfromip"].ToString());
                    }                    
                }                
            }
            dataReader.Close();
            connection.Close();
            
            // Remove all allowed rules from firewall
            RemoveFirewallRule("BreakermindCom");
            // Add all refreshed rules
            foreach (var ip in AllowedIPAddress)
            {
                AddFirewallRule(ip,"BreakermindCom");
            }            
        }

        // Update list with allowed ip addresses
        public static void UpdateBanIP()
        {
            List<string> BanIPAddress = new List<string>();            
            MySqlConnection connection = new MySqlConnection(con);
            connection.Open();
            string query = "SELECT banip FROM banip;";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            //cmd.Parameters.AddWithValue("@User", user);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                dataReader["banip"].ToString().Replace("/", "");
                if (dataReader["banip"].ToString() != null && dataReader["banip"].ToString() != "" && dataReader["banip"].ToString() != "0.0.0.0")
                {
                    if (IsIPv4(dataReader["banip"].ToString()))
                    {
                        BanIPAddress.Add(dataReader["banip"].ToString());
                    }
                }
            }
            dataReader.Close();
            connection.Close();

            // Remove all Ban rules
            RemoveFirewallRule("BreakermindCom", true);
            // Add all new Ban rules
            foreach (var ip in BanIPAddress)
            {
                // Add not allow rule
                AddFirewallRule(ip,"BreakermindCom",true);
            }
            // AddFirewallRule("89.230.40.121");
            // AddFirewallRule("89.230.40.131");
            // Remove all rules with TEXT
            // RemoveFirewallRule("BreakermindCom");
        }

        public void start()
        {
            Thread t = new Thread(() => {
                while (true)
                {                    
                    UpdateAllowedIP();
                    UpdateBanIP();
                    Thread.Sleep(1000 * 60);
                }
            });
            t.Start();

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(HostIP), Port);
            TcpListener listener = new TcpListener(endpoint);
            listener.Start();

            while (true)
            {
                tcpClientConnected.Reset();
                listener.BeginAcceptTcpClient(new AsyncCallback(ProcessIncomingConnection), listener);
                tcpClientConnected.WaitOne();
            }
        }

        static void DisplayCertificateInformation(SslStream stream)
        {
            Console.WriteLine("Certificate revocation list checked: {0}", stream.CheckCertRevocationStatus);

            Console.WriteLine("Cipher: {0} strength {1}", stream.CipherAlgorithm, stream.CipherStrength);
            Console.WriteLine("Hash: {0} strength {1}", stream.HashAlgorithm, stream.HashStrength);
            Console.WriteLine("Key exchange: {0} strength {1}", stream.KeyExchangeAlgorithm, stream.KeyExchangeStrength);
            Console.WriteLine("Protocol: {0}", stream.SslProtocol);

            X509Certificate localCertificate = stream.LocalCertificate;
            if (stream.LocalCertificate != null)
            {
                Console.WriteLine("Local cert was issued to {0} and is valid from {1} until {2}.",
                    localCertificate.Subject,
                    localCertificate.GetEffectiveDateString(),
                    localCertificate.GetExpirationDateString());
            }
            else
            {
                Console.WriteLine("Local certificate is null.");
            }
            // Display the properties of the client's certificate.
            X509Certificate remoteCertificate = stream.RemoteCertificate;
            if (stream.RemoteCertificate != null)
            {
                Console.WriteLine("Remote cert was issued to {0} and is valid from {1} until {2}.",
                    remoteCertificate.Subject,
                    remoteCertificate.GetEffectiveDateString(),
                    remoteCertificate.GetExpirationDateString());
            }
            else
            {
                Console.WriteLine("Remote certificate is null.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Multi user server. Recive save and send data to clients max 10 accounts.");
            //DateTime.Now.ToLongTimeString()
            Console.WriteLine(DateTime.Now + " Waiting for connections....");
            try
            {
                server s = new server();
                s.start();
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
                Console.ReadKey();
            }
        }
    }
}

