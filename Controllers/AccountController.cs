using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuctionWeb.Models;
using AuctionWeb.Helpers;
using BotDetect.Web.Mvc;
using AuctionWeb.Filters;

namespace AuctionWeb.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
            string encPwd = StringUtils.Md5(model.f_RawPassword);
            using (var ctx = new AuctionSiteDBEntities())
            {
                var user = ctx.Users.Where(u => u.Username == model.f_Username && u.Password == encPwd)
                    .FirstOrDefault();
                if(user != null)
                {
                    Session["isLogin"] = 1;
                    Session["user"] = user;

                    if(model.Remember)
                    {
                        Response.Cookies["userId"].Value = user.ID.ToString();
                        Response.Cookies["userId"].Expires = DateTime.Now.AddDays(7);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMsg = "Login failed! check your information...";
                    return View();
                }
            }
        }

        // POST: Account/Logout
        [HttpPost]
        public ActionResult Logout()
        {
            CurrentContext.Destroy();
            return RedirectToAction("Index", "Home");
        }


        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }


        // POST: Account/Register
        [HttpPost]
        [CaptchaValidation("CaptchaCode", "ExampleCaptcha", "Incorrect CAPTCHA code!")]
        public ActionResult Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMsg = "Incorrect CAPTCHA code!";
            }
            else
            {

                User u = new User
                {
                    Username = model.f_Username,
                    Email = model.f_Email,
                    Name = model.f_Name,
                    Address = model.f_Address,
                    Password = StringUtils.Md5(model.f_RawPassword),
                    Permission = 0,
                    DOB = DateTime.ParseExact(model.f_DOB, "d/M/yyyy", null)
                };

                using (var ctx = new AuctionSiteDBEntities())
                {
                    ctx.Users.Add(u);
                    ctx.SaveChanges();
                }              
            }
            return View();
        }

        // GET: /Account/Profile
        [CheckLogin]
        public ActionResult Profile()
        {
            if(CurrentContext.Islogged() == false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

    }
}