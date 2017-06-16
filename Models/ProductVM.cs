using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuctionWeb.Models
{
    public class ProductVM
    {
        public int ID { get; set; }
        public int IDCat { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> StartPrice { get; set; }
        public Nullable<decimal> StepPrice { get; set; }
        public Nullable<decimal> EndPrice { get; set; }
        public int IntervalTime { get; set; }
        public Nullable<bool> ExtendTime { get; set; }
        public Nullable<double> EvaluationPoint { get; set; }
        public Nullable<int> HighestKeeper { get; set; }
        public System.DateTime TimePost { get; set; }
        public Nullable<decimal> CurrentPrice { get; set; }
        public int UserID { get; set; }
        public Nullable<bool> Bought { get; set; }
    }
}