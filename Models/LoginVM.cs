using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuctionWeb.Models
{
    public class LoginVM
    {
        public string f_Username { get; set; }
        public string f_RawPassword { get; set; }
        public bool Remember { get; set; }
    }
}