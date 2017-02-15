using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectApi
{
    class ClassAPi
    {
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
}
