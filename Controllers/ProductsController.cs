using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuctionWeb.Models;

namespace AuctionWeb.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products/ByCat
        public ActionResult ByCat(int? id)
        {
            if(id.HasValue == false)
            {
                return RedirectToAction("Index", "Home");
            }

            using (var ctx = new AuctionSiteDBEntities())
            {
                var list = ctx.Products.Where(p => p.IDCat == id)
                    .ToList();
                return View(list);
            }
        }

        // GET: Products/Details
        public ActionResult Details(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Home");
            }

            using (var ctx = new AuctionSiteDBEntities())
            {
                var model = ctx.Products
                    .Where(p => p.ID == id)
                    .FirstOrDefault();

                return View(model);
            }
        }
    }
}