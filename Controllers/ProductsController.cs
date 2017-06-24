using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuctionWeb.Models;
using AuctionWeb.Helpers;
using System.Data.Entity.Core.Objects;
using Postal;

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
                if (listpros.Count > 0)
                {
                    foreach (Product pro in listpros)
                    {
                        dynamic emailforwinner = new Email("win");
                        dynamic emailforonwer = new Email("owner");
                        pro.Bought = true;
                        if(pro.lastuser != null)
                        {
                            var user = ctx.Users.Where(u => u.ID == pro.lastuser).FirstOrDefault();
                            var userowner = ctx.Users.Where(u => u.ID == pro.UserID).FirstOrDefault();
                            //create an email       
                            //for winnner
                            emailforwinner.To = user.Email;
                            emailforwinner.Name = user.Name;
                            emailforwinner.ProName = pro.Name;
                            emailforwinner.price = pro.PriceDisplay;
                            emailforwinner.Send();
                            //for onwer
                            emailforonwer.To = userowner.Email;
                            emailforonwer.Name = userowner.Name;
                            emailforonwer.ProName = pro.Name;
                            emailforonwer.price = pro.PriceDisplay;
                            emailforwinner.Send();
                        }
                    }
                }
                ctx.SaveChanges();
                bool list = ctx.Products.Any(p => p.Bought == false && p.IDCat == id);
                var listpro = ctx.Products.Where(p => p.Bought == false && p.IDCat == id).ToList();
                //check if no product is found because all of them have been deleted at check expired
                if (list == true)
                {
                    return View(listpro);
                }
                else
                {
                    return View(model: null);
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
        // GET: Products/Search
        public ActionResult Search(int? id)
        {
            return View();
        }
    }
}
