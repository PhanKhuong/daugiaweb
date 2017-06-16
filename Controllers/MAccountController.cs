using AuctionWeb.Filters;
using AuctionWeb.Helpers;
using AuctionWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuctionWeb.Controllers
{
    [CheckLogin(RequiredPermission = 0)]
    public class MAccountController : Controller
    {
        // GET: MAccount
        public ActionResult Index()
        {
            return View();
        }

        // GET: Account/AskPermission
        [HttpPost]
        public ActionResult AskPermission()
        {
            var WaitingUser = new UsersAsking()
            {
                Username = CurrentContext.GetCurUser().Username,
                Email = CurrentContext.GetCurUser().Email,
                Name = CurrentContext.GetCurUser().Name,
                AskingDate = DateTime.Now,
                IDUser = CurrentContext.GetCurUser().ID,
            };

            using (var ctx = new AuctionSiteDBEntities())
            {            
                ctx.UsersAskings.Add(WaitingUser);
                ctx.SaveChanges();
            }

            return View();
        }

        // GET: MAccount
        public ActionResult WaitingUsers()
        {
            using (var ctx = new AuctionSiteDBEntities())
            {
                var list = (from u in ctx.Users
                            join us in ctx.UsersAskings
                            on u.ID equals us.IDUser
                            select us).ToList();

                return View(list);
            }
        }

        // GET: Delete
        public ActionResult Delete()
        {
            using (var ctx = new AuctionSiteDBEntities())
            {
                var curnameuser = CurrentContext.GetCurUser().Username;
                var list = ctx.Users.Where(u => u.Username != curnameuser).ToList();

                return View(list);
            }
        }

        // Post: Delete
        [HttpPost]
        public ActionResult Delete(User vm)
        {
            using (var ctx = new AuctionSiteDBEntities())
            {
                var user = new User { ID = vm.ID };
                ctx.Users.Attach(user);
                ctx.Users.Remove(user);
                ctx.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
        }

        // Post: Reset
        [HttpPost]
        public ActionResult Reset(User vm)
        {
            using (var ctx = new AuctionSiteDBEntities())
            {
                var list = ctx.Users.ToList();
                User usertoUpdate = list.Where(u => u.ID == vm.ID).FirstOrDefault<User>();
                usertoUpdate.Password = StringUtils.Md5(vm.Email);
                ctx.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }
    }
}