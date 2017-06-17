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
                var list_auction = ctx.Auctions.ToList();
                var list_product = ctx.Products.ToList();
                var user = CurrentContext.GetCurUser();
                var pro = ctx.Products.Where(p => p.ID == vm.ID).FirstOrDefault();         
                var takenew = ctx.Auctions.Where(t => t.own == true).FirstOrDefault();
                if (list_auction.Count == 0)
                {
                    takenew.MaxPrice = 0;
                }
                if ((vm.Price >= (pro.PriceDisplay + pro.StepPrice)) && (vm.Price < (takenew.MaxPrice - pro.StepPrice)))
                {
                    //product
                    var product = list_product.Where(p => p.ID == pro.ID).FirstOrDefault<Product>();
                    product.PriceDisplay = vm.Price + pro.StepPrice;                    
                    //create a new one
                    var ac = new Auction()
                    {
                        IDPro = pro.ID,
                        IDUser = pro.UserID,
                        Username = user.Username,
                        Fullname = user.Name,
                        Time = DateTime.Now,
                        MaxPrice = vm.Price,
                        own = false,
                    };
                    ctx.Auctions.Add(ac);
                    ctx.SaveChanges();
                }
                else if((vm.Price >= (pro.PriceDisplay + pro.StepPrice)))
                {
                    pro.PriceDisplay += pro.StepPrice;
                    pro.CurrentPrice = vm.Price;

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
                    ctx.Auctions.Add(ac);
                    ctx.SaveChanges();
                }
            }
            return View();
        }
    }
}