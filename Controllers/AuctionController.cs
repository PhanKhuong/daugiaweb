using AuctionWeb.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuctionWeb.Models;
using AuctionWeb.Helpers;

namespace AuctionWeb.Controllers
{
    [CheckLogin]
    public class AuctionController : Controller
    {
        // GET: Auction
        public ActionResult Index(BargainVM vm)
        {
            using (var ctx = new AuctionSiteDBEntities())
            {
                var list_product = ctx.Products.ToList();
                var list_auction = ctx.Auctions.ToList();
                var user = CurrentContext.GetCurUser();
                var pro = ctx.Products.Where(p => p.ID == vm.ID).FirstOrDefault<Product>();
                //check if user just set a price for a product
                if(pro.lastuser != CurrentContext.GetCurUser().ID
                   && (vm.Price >= pro.PriceDisplay + pro.StepPrice))
                {
                    //check if each product has been setted a price once time
                    if ((list_auction.Any(l => l.IDPro == pro.ID) == false))
                    {
                        var ac = new Auction()
                        {
                            IDPro = pro.ID,
                            IDUser = pro.UserID,
                            Username = user.Username,
                            Fullname = user.Name,
                            Time = DateTime.Now,
                            MaxPrice = vm.Price,
                            own = true,
                        };
                        pro.lastuser = user.ID;
                        pro.PriceDisplay = ac.MaxPrice;
                        ctx.Auctions.Add(ac);
                        ctx.SaveChanges();
                        ViewBag.info = "successfully!!!";
                    }
                    else
                    {
                        var takeowner = ctx.Auctions.Where(t => t.own == true && t.IDPro == pro.ID).FirstOrDefault();
                        var ac = new Auction()
                        {
                            IDPro = pro.ID,
                            IDUser = pro.UserID,
                            Username = user.Username,
                            Fullname = user.Name,
                            Time = DateTime.Now,
                            MaxPrice = vm.Price,
                        };
                        if (takeowner.MaxPrice < ac.MaxPrice)
                        {
                            ac.own = true;
                        }
                        pro.lastuser = user.ID;
                        pro.PriceDisplay = ac.MaxPrice;
                        takeowner.own = false;
                        ctx.Auctions.Add(ac);
                        ctx.SaveChanges();
                        ViewBag.info = "successfully!!!";
                    }
                }
                else
                {
                    ViewBag.info = "You just nailled a price for it";
                }               
            }
            return RedirectToAction("Details", "Products", new {id = vm.ID});
        }
    }
}