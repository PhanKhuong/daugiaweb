﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuctionWeb.Models;
using System.Data.Entity.Core.Objects;
using AuctionWeb.Helpers;
namespace AuctionWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        private object CurrentContext()
        {
            throw new NotImplementedException();
        }
    }
}