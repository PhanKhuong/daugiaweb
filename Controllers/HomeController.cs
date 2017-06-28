using System;
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
            List<Product> SetPrice = new List<Product>();
            using (var ctx = new AuctionSiteDBEntities())
            {
                var listget = ctx.Auctions.ToList();
                var listmax = ctx.Products.Where(p => p.Bought == false && (DateTime.Now <= EntityFunctions.AddDays(p.TimePost, p.IntervalTime))).OrderByDescending(p => p.PriceDisplay).ToList().Take(5);
                var listend = ctx.Products.Where(p => p.Bought == false && (DateTime.Now <= EntityFunctions.AddDays(p.TimePost, p.IntervalTime)))
                    .OrderBy(p => (EntityFunctions.DiffSeconds(DateTime.Now, EntityFunctions.AddDays(p.TimePost, p.IntervalTime)))).ToList().Take(5);
                var TopSetPriceList = listget.GroupBy(a => a.IDPro).OrderByDescending(a => a.Key).ToList().Take(5);             
                foreach (var id in TopSetPriceList)
                {
                    Product pro = ctx.Products.Where(p => p.ID == id.Key && p.Bought == false).FirstOrDefault();
                    if(pro != null)
                    {
                        SetPrice.Add(pro);
                    }                  
                }

                HomeVM vm = new HomeVM();
                vm.MostSettingPrice = new List<Product>();
                vm.MaxPrice = new List<Product>();
                vm.EndDate = new List<Product>();
                vm.EndDate.AddRange(listend);
                vm.MostSettingPrice.AddRange(SetPrice);
                vm.MaxPrice.AddRange(listmax);
                return View(vm);
            }         
        }

        private object CurrentContext()
        {
            throw new NotImplementedException();
        }
    }
}
