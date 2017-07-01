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
                if(listpros.Count > 0)
                {
                    foreach (Product pro in listpros)
                    {
                        pro.Bought = true;
                    }
                }              
                ctx.SaveChanges();
                bool list = ctx.Products.Any(p => p.Bought == false);
                var listpro = ctx.Products.Where(p => p.Bought == false).ToList();
                //check if no product is found because all of them have been deleted at check expired
                if(list == true)
                {
                    return View(listpro);
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