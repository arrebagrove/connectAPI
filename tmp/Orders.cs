using System;
using System.Collections.Generic;

namespace ClientSSL
{
    // Get signals
    class OpenSignals
    {
        public int uLogin { get; set; }
        public string uPass { get; set; }
        private int copyAccount { get; set; }
        private string Cmd { get; set; }
        private DateTime Time { get; set; }
        private Int32 Timestamp { get; set; }
        public List<Order> Orders = new List<Order>();

        // Konstruktor
        public OpenSignals()
        {
            this.uLogin = 0;
            this.uPass = "0";
            this.copyAccount = 0;
            this.Cmd = "GETOPEN";
            this.Time = DateTime.UtcNow;
            this.Timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0))).TotalSeconds;
            this.Orders.Clear();
        }
    }

    //Get close signals
    class CloseSignals
    {
        public int aLogin { get; set; }
        public string aPass { get; set; }
        private int copyAccount { get; set; }
        private string Cmd { get; set; }
        private DateTime Time { get; set; }
        private Int32 Timestamp { get; set; }
        public List<OrderClose> Orders = new List<OrderClose>();

        // Konstruktor
        public CloseSignals()
        {
            this.aLogin = 0;
            this.aPass = "0";
            this.copyAccount = 0;
            this.Cmd = "GETCLOSE";
            this.Time = DateTime.UtcNow;
            this.Timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0))).TotalSeconds;
            this.Orders.Clear();
        }
    }


    // Send positions
    class MsgADD
    {
        public int aLogin { get; set; }
        public string aPass { get; set; }
        private string Cmd { get; set; }
        private DateTime Time { get; set; }
        private Int32 Timestamp { get; set; }
        public List<Order> Orders = new List<Order>();

        // Konstruktor
        public MsgADD()
        {
            this.aLogin = 0;
            this.aPass = "0";
            this.Cmd = "ADDOPEN";
            this.Time = DateTime.UtcNow;
            this.Timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0))).TotalSeconds;
            this.Orders.Clear();
        }
    }

    class MsgCLOSE
    {
        public int aLogin { get; set; }
        public string aPass { get; set; }
        private string Cmd { get; set; }
        private DateTime Time { get; set; }
        private Int32 Timestamp { get; set; }
        public List<OrderClose> Orders = new List<OrderClose>();

        // Konstruktor
        public MsgCLOSE()
        {
            this.aLogin = 0;
            this.aPass = "0";
            this.Cmd = "ADDCLOSE";
            this.Time = DateTime.UtcNow;
            this.Timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0))).TotalSeconds;
            this.Orders.Clear();
        }
    }


    // Open positions class
    class Order
    {
        public string id, type;
        public DateTime time;
        public double lot, price, sl, tp;

        public Order(string id, string type, double lot, double price, double sl, double tp, DateTime time)
        {
            this.id = id;
            this.type = type;
            this.lot = lot;
            this.price = price;            
            this.sl = sl;
            this.tp = tp;
            this.time = time;
        }
    }

    // Closed positions class - History positions
    class OrderClose
    {
        public string id, type, time, timeclose;
        public double lot, price, priceclose, sl, tp;

        public OrderClose(string id, string type, double lot, double price, double priceclose, double sl, double tp, string time, string timeclose)
        {
            this.id = id;
            this.type = type;
            this.lot = lot;
            this.price = price;
            this.priceclose = priceclose;
            this.sl = sl;
            this.tp = tp;
            this.time = time;
            this.timeclose = timeclose;
        }
    }
}
