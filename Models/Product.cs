//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AuctionWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
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
        public Nullable<decimal> PriceDisplay { get; set; }
        public Nullable<int> lastuser { get; set; }
    }
}
