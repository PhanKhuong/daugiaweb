using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuctionWeb.Models;

namespace AuctionWeb.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: Categories/ListOfCats
        public ActionResult ListOfCats()
        {
            using (var ctx = new AuctionSiteDBEntities())
            {
                var List = ctx.Categories.ToList();
                return PartialView("ListOfCats", List);
            }
        }
    }
}