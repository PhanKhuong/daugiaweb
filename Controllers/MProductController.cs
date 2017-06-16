using AuctionWeb.Filters;
using AuctionWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuctionWeb.Controllers
{
    [CheckLogin(RequiredPermission=1)]
    public class MProductController : Controller
    {
        // GET: MProduct
        public ActionResult Index()
        {
            return View();
        }

        // GET: MProduct/Add
        public ActionResult Add()
        {
            using (var ctx = new AuctionSiteDBEntities())
            {
                var list = ctx.Categories.ToList();
                ViewBag.Categories = list;
            }

            return View();
        }
        
        [HttpPost]
        public ActionResult Add(Product vm, HttpPostedFileBase Firstimg, HttpPostedFileBase Secondimg
            , HttpPostedFileBase Thirdimg)
        {
            using (var ctx = new AuctionSiteDBEntities())
            {
                vm.CurrentPrice = null;
                vm.HighestKeeper = null;
                vm.EvaluationPoint = null;
                vm.TimePost = DateTime.Now;
                ctx.Products.Add(vm);
                ctx.SaveChanges();

                if(Firstimg != null && Firstimg.ContentLength > 0 && (Secondimg != null && Secondimg.ContentLength > 0) &&
                    (Thirdimg != null && Thirdimg.ContentLength > 0))
                {
                    string spDirPath = Server.MapPath("~/Img/products");
                    string targetDirPath = Path.Combine(spDirPath, vm.ID.ToString());
                    Directory.CreateDirectory(targetDirPath);

                    string mainFileName = Path.Combine(targetDirPath, "main.jpg");
                    Firstimg.SaveAs(mainFileName);

                    string mainFileName2 = Path.Combine(targetDirPath, "main2.jpg");
                    Secondimg.SaveAs(mainFileName2);

                    string mainFileName3 = Path.Combine(targetDirPath, "main3.jpg");
                    Thirdimg.SaveAs(mainFileName3);
                }

                var list = ctx.Categories.ToList();
                ViewBag.Categories = list;
            }

            return View();
        }

        // GET: MProduct/InTime
        public ActionResult InTime()
        {
            using (var ctx = new AuctionSiteDBEntities())
            {
                var list = ctx.Products.Where(p => (DateTime.Now <= EntityFunctions.AddDays(p.TimePost, p.IntervalTime)))
                .ToList();
                return View(list);
            }          
        }

        // GET: MProduct/WatchedList
        public ActionResult WatchList()
        {
            return View();
        }
    }
}