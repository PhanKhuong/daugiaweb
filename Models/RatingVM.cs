using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuctionWeb.Models
{
    public class RatingVM
    {
        public int ID { get; set; }
        public double negativePoint { get; set; }
        public double positivePoint { get; set; }
    }
}