// System
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
// cAlgo

using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Newtonsoft.Json;

namespace ClientSSL
{
    class Program
    {
        // "Server SSL Certyficate (CN=www.domain.com)"
        public static string hostname = "www.fxstar.eu";

        // "Server host www.example.com"
        public static string host = "0.0.0.0";

        // "Server port"
        public static int port = 1234;

        List<string> PosOpenID = new List<string>();

        public static string txt = "";

        private static Hashtable certificateErrors = new Hashtable();

        // The following method is invoked by the RemoteCertificateValidationDelegate.
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }else
            {
                return false;
            }
            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            //return false;
            //Force ssl certyfikates as correct set true
            // return false;
        }

        static string ReadMessage(SslStream sslStream)
        {
            // Read the  message sent by the server. 
            // The end of the message is signaled using the 
            // "<||>" marker.
            byte[] buffer = new byte[2048];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;
            do
            {
                bytes = sslStream.Read(buffer, 0, buffer.Length);

                // Use Decoder class to convert from bytes to UTF8 
                // in case a character spans two buffers.
                Decoder decoder = Encoding.UTF8.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);
                // Check for ||. 
                if (messageData.ToString().IndexOf("<||>") != -1)
                {
                    break;
                }
            } while (bytes != 0);

            return messageData.ToString();
        }

        static void DisplayCertificateInformation(SslStream stream)
        {
            Console.WriteLine("Certificate revocation list checked: {0}", stream.CheckCertRevocationStatus);

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

        // Get random color
        public static ConsoleColor getRandomColor()
        {
            return (ConsoleColor)(new Random().Next(1,16));
        }

        // Connect
        public static void ConnectSSL(string msg = "")
        {
            txt = "";
            try
            {
                TcpClient client = new TcpClient(host, port);

                // Create an SSL stream that will close the client's stream.
                SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                try
                {
                    sslStream.AuthenticateAsClient(hostname);
                    //DisplayCertificateInformation(sslStream);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    client.Close();
                    return;
                }

                // Signal the end of the message using the "<||>".
                byte[] messsage = Encoding.UTF8.GetBytes(msg + "<||>");
                sslStream.Write(messsage);
                sslStream.Flush();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Send message: " + msg);
                // Console.ForegroundColor = ConsoleColor.Green;
                Console.ForegroundColor = getRandomColor();
                string serverMessage = ReadMessage(sslStream);
                Console.WriteLine(DateTime.UtcNow + " " + serverMessage);                
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e.ToString());
            }
            catch (Exception e)
            {
                //Console.WriteLine("SocketException: {0}", e.ToString());
            }
        }        

        static void Main(string[] args)
        {
            // string output = JsonConvert.SerializeObject(Positions);
            // MsgADD account = JsonConvert.DeserializeObject<MsgADD>(output);

            try
            {
                while(true)
                {
                    Int64 t1 = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0))).TotalMilliseconds;
                    
                    MsgGet Msg = new MsgGet();
                    Msg.User = "Mindbreaker";
                    Msg.Pass = "pass";
                    //Msg.accountId = 424860;
                    Msg.accountId = 431708;

                    string sendMsg = JsonConvert.SerializeObject(Msg);
                    Console.WriteLine(sendMsg);
                    // Console.ForegroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = getRandomColor();
                    ConnectSSL(sendMsg);
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Int64 t2 = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0))).TotalMilliseconds;
                    Console.WriteLine("Request time (send) " + t1 + " (receive) " + t2 + " (t2-t1) " + (t2-t1).ToString() + "ms" );
                    Thread.Sleep(1000);
                }
            }
            catch (Exception h)
            {
                Console.WriteLine("Coś się mu pomieszało :]" + h);
            }
        }
    }
}
