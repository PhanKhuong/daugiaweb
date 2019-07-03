using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuctionWeb.Models;
using AuctionWeb.Helpers;
using System.Data.Entity.Core.Objects;

namespace AuctionWeb.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products/ByCat
        public ActionResult ByCat(int? id)
        {

            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Home");
            }

            using (var ctx = new AuctionSiteDBEntities())
            {
                //check expired
                var listpros = ctx.Products.Where(p => (DateTime.Now > System.Data.Entity.DbFunctions.AddDays(p.TimePost, p.IntervalTime)))
                    .ToList();
                ctx.Products.RemoveRange(listpros);
                ctx.SaveChanges();
                var list = ctx.Products.Where(p => p.IDCat == id)
                    .ToList();
                //check if no product is found because all of them have been deleted at check expired
                if(list.Count > 0)
                {
                    return View(list);
                }
                else
                {
                    return View(model:null);
                }
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
                //check band
                if (CurrentContext.Islogged() == true)
                {
                    int iduser = CurrentContext.GetCurUser().ID;
                    var checkbanned = ctx.BannedUsers.Any(p => p.IDProduct == id && p.IDUser == iduser);
                    ViewBag.CheckBanned = checkbanned;
                }
                    return View(model);
            }
        }
    }
}