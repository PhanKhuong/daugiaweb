using AuctionWeb.Filters;
using AuctionWeb.Helpers;
using AuctionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuctionWeb.Controllers
{
    [CheckLogin(RequiredPermission = 1)]
    public class MCategoriesController : Controller
    {
        // GET: MCategories
        public ActionResult Index()
        {
            using (var ctx = new AuctionSiteDBEntities())
            {
                var list = ctx.Categories.ToList();
                return View(list);
            }
        }

        // Post: MCategories/Add
        [HttpPost]
        public ActionResult Add(Category vm)
        {
            using (var ctx = new AuctionSiteDBEntities())
            {
                var cat = new Category()
                {
                    Name = vm.Name,
                };

                ctx.Categories.Add(cat);
                ctx.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }

        // Post: MCategories/Edit
        [HttpPost]
        public ActionResult Edit(Category vm)
        {
            using (var ctx = new AuctionSiteDBEntities())
            {
                var list = ctx.Categories.ToList();
                Category CattoUpdate = list.Where(c => c.ID == vm.ID).FirstOrDefault<Category>();
                CattoUpdate.Name = vm.Name;
                ctx.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }

        // Post: MCategories/Delete
        [HttpPost]
        public ActionResult Delete(Category vm)
        {
            using (var ctx = new AuctionSiteDBEntities())
            {
                var cat = new Category { ID = vm.ID };
                ctx.Categories.Attach(cat);
                ctx.Categories.Remove(cat);
                ctx.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
        }
    }
}