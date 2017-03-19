using System;
using System.Collections.Generic;

namespace server
{


    // Get positions from account id
    class MsgGet
    {
        public string User { get; set; }
        public string Pass { get; set; }
        public Int64 accountId { get; set; } = 0;
        // GETOPEN, GETCLOSE, GETALL
        public string Cmd { get; } = "GETOPEN";
        public Int64 Timestamp { get; set; } = 0;

        // Konstruktor
        public MsgGet()
        {
            this.User = "";
            this.Pass = "";
            this.accountId = 0;
            this.Timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0))).TotalMilliseconds;
        }
    }

    // Open positions class (list)
    class OpenOrders
    {
        public List<OrderOpen> OpenPositions = new List<OrderOpen>();
        public List<OrderClose> ClosePositions = new List<OrderClose>();
    }

    // Open position class
    class OrderOpen
    {
        public string accountId { get; set; }
        public string positionId { get; set; }
        public string symbolName { get; set; }
        public string tradeSide { get; set; }
        public string volume { get; set; }
        public string entryTimestamp { get; set; }
        public string timestamp { get; set; }
    }

    // Closed positions class - History positions
    class OrderClose
    {
        public string accountId { get; set; }
        public string positionId { get; set; }
        public string symbolName { get; set; }
        public string tradeSide { get; set; }
        public string volume { get; set; }
        public string timestamp { get; set; }
    }


    // Get signals
    class OpenSignals
    {
        public int uLogin { get; set; }
        public string uPass { get; set; }
        private int copyAccount { get; set; }
        private string Cmd { get; set; }
        private DateTime Time { get; set; }
        private Int32 Timestamp { get; set; }
        public List<OrderOpen> Orders = new List<OrderOpen>();

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
        public List<OrderOpen> Orders = new List<OrderOpen>();

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

}
