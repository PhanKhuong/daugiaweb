using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuctionWeb.Models
{
    public class HomeVM
    {
        public List<Product> MostSettingPrice { get; set; }
        public List<Product> MaxPrice { get; set; }
        public List<Product> EndDate { get; set; }
    }
}