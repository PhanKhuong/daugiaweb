using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuctionWeb.Models
{
    public class RegisterVM
    {
        public string f_Username { get; set; }
        public string f_RawPassword { get; set; }
        public string f_Name { get; set; }
        public string f_Email { get; set; }
        public string f_DOB { get; set; }
        public string f_Address { get; set; }
    }
}