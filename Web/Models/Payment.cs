using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YuChang.Web.Models
{
    public class Payment
    {
        public string OpenId { get; internal set; }

        public string AppId { get; internal set; }

        public string AppSignature { get; internal set; }

        public bool IsSubscribe { get; internal set; }

        public DateTime TimeEnd { get; internal set; }

        public string NonceStr { get; internal set; }

        //public string Sign { get; internal set; }

        public int TotalFee { get; internal set; }

        public string OutTradeNO { get; internal set; }

        public string TransactionId { get; internal set; }

        public string BankType { get; internal set; }

        public int CashFee { get; internal set; }

        public string FeeType { get; internal set; }

        public string MchId { get; internal set; }
    }
}