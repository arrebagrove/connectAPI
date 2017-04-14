using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// Add Json dll from Newton.Json
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Json
{
    class Program
    {
        public static string json = "";

        static void Main(string[] args)
        {
            // ===============================================
            // Parse Json string from file to object
            // {"name":"FxstarJson","email":["superemail@fxstar.eu","one@fxstar.eu"],"websites":{"home page":"http:\/\/fxstar.eu","blog":"http:\/\/blog.fxstar.eu"}}
            // ===============================================
            using (StreamReader sr = new StreamReader("../../json.txt"))
            {                
                String json = sr.ReadToEnd();
                // Parse to Jobject
                JObject o = JObject.Parse(json);

                // Write to console
                Console.WriteLine(json);
                Console.WriteLine("Name: " + o["name"]);
                Console.WriteLine("First email address: " + o["email"][0]);
                Console.WriteLine("Website [blog]: " + o["websites"]["blog"]);
            }
            // wait for key
            Console.ReadKey();

            // ===============================================
            // Get json from web page txt file or php file to object
            // {"name":"FxstarJson","email":["hello@fxstar.eu","one@fxstar.eu"],"websites":{"home page":"http:\/\/fxstar.eu","blog":"http:\/\/blog.fxstar.eu"}}
            // ===============================================
            WebClient c = new WebClient();
            var jsondata = c.DownloadString("https://fxstar.eu/json.php");

            // Parse to Jobject
            JObject jo = JObject.Parse(jsondata);

            // Write to console
            Console.WriteLine(jsondata);
            Console.WriteLine("Name: " + jo["name"]);
            Console.WriteLine("First email address: " + jo["email"][0]);
            Console.WriteLine("Website [blog]: " + jo["websites"]["blog"]);
            // wait for key
            Console.ReadKey();


            // ===============================================
            // Deserialize Json String
            // ===============================================
            string jdata = @"{'data':[{'positionId':7980418,'entryTimestamp':1487151223210,'utcLastUpdateTimestamp':1487151223210,'symbolName':'GBPUSD','tradeSide':'BUY','entryPrice':1.2428,'volume':100000,'stopLoss':null,'takeProfit':null,'profit':-338,'profitInPips':-35.7,'commission':-3,'marginRate':1.17765,'swap':0,'currentPrice':1.23923,'comment':null,'channel':'cAlgo','label':null},{'positionId':123456,'entryTimestamp':1487151223210,'utcLastUpdateTimestamp':1487151223210,'symbolName':'GBPUSD','tradeSide':'BUY','entryPrice':1.2428,'volume':100000,'stopLoss':null,'takeProfit':null,'profit':-338,'profitInPips':-35.7,'commission':-3,'marginRate':1.17765,'swap':0,'currentPrice':1.23923,'comment':null,'channel':'cAlgo','label':null}], 'id': 123, 'name': 'Henio'}";

            // Deserializestring to dynamic object class
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(jdata);

            // Write to console
            Console.WriteLine(obj.data[0].positionId);
            Console.WriteLine(obj.data[1].positionId);
            Console.WriteLine(obj.id);
            Console.WriteLine(obj.name);
            // wait for key
            Console.ReadKey();

            // ===============================================
            // Serialize Json String from class with 
            // List with positions
            // ===============================================
            try
            {
                // Create first position
                Position pos1 = new Position();
                pos1.positionId = "123";
                pos1.symbolName = "GBPJPY";
                pos1.entryTimestamp = "1499900011";

                // Create second position
                Position pos2 = new Position();
                pos2.positionId = "456";
                pos2.symbolName = "PLNJPY";
                pos2.entryTimestamp = "1499955611";

                // Create list with 2 positions
                List<Position> List = new List<Position>();
                List.Add(pos1);
                List.Add(pos2);

                // Create AllPositions object and add text and List with positions
                AllPositions positions = new AllPositions();
                positions.name = "Fxstar";
                positions.id = 123456;
                positions.data = List;
                
                // Serialize class
                string outJson = JsonConvert.SerializeObject(positions, Formatting.None);
                
                // Write to console
                Console.WriteLine("\r\n Serialize Class to Json: " + outJson);
                // wait for key
                Console.ReadKey();

            }catch(Exception e){
                // Write to console
                Console.WriteLine(e.ToString());
                // wait for key
                Console.ReadKey();
            }
            
        }


        // Json object class
        public class AllPositions
        {
            public List<Position> data { get; set; }
            public string name { get; set; }
            public int id { get; set; }
        }

        public class Position
        {
            public string positionId { get; set; }
            public string entryTimestamp { get; set; }
            public string symbolName { get; set; }
        }
    }
}
