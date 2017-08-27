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

                    if(user.AskingDate != null)
                    {
                        if (user.AskingDate.Value.AddDays(7) < DateTime.Now)
                        {
                            var list = ctx.Users.ToList();
                            User usertoUpdate = list.Where(u => u.ID == user.ID).FirstOrDefault<User>();
                            usertoUpdate.Permission = 0;
                            ctx.SaveChanges();
                        }
                    }                   
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMsg = "Login failed! check your information!!!";
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
            using (var ctx = new AuctionSiteDBEntities())
            {
                var list = ctx.Users.ToList();
                if(list.Any(u => u.Email == model.f_Email))
                {
                    ViewBag.ErrorMsg = "this email has been already used ";
                    return View();
                }
                if( list.Any(u => u.Username == model.f_Username))
                {
                    ViewBag.ErrorMsg = "this username has been already used ";
                    return View();
                }               
            }

                if (!ModelState.IsValid)
            {
                ViewBag.ErrorMsg = "Incorrect CAPTCHA code!";
            }
            else
            {

                User u = new User
                {
                    IsBargain = false,
                    AskingDate = null,
                    positivePoint = 0,
                    negativePoint = 0,
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
            ViewBag.suss = "Cheers! sign up successfully. Please login to keep track your works!!!";
            return View();
        }

        // GET: /Account/Profile
        [CheckLogin]
        public ActionResult Profile()
        {
            User user = CurrentContext.GetCurUser();
            double total_percents = Math.Abs(user.positivePoint / (user.positivePoint + user.negativePoint)) * 100;
            ViewBag.Point = total_percents;
                using (var ctx = new AuctionSiteDBEntities())
                {
                    var iduser = CurrentContext.GetCurUser().ID;
                    var list_rating = ctx.Ratings.Where(p => p.IDUser == iduser).ToList();
                    return View();
                }                    
        }

        //Change info
        [CheckLogin]
        public ActionResult ChangeInfo()
        {
            int id = CurrentContext.GetCurUser().ID;
            using (var ctx = new AuctionSiteDBEntities())
            {
                var user = ctx.Users.Where(u => u.ID == id).FirstOrDefault();
                return View(user);
            }              
        }

        //Change info
        [CheckLogin]
        [HttpPost]
        public ActionResult ChangeInfo(User vm)
        {            
            using (var dt = new AuctionSiteDBEntities())
            {
                string encPwd = StringUtils.Md5(vm.Password);
                var list = dt.Users.ToList();
                var user = dt.Users.Where(u => u.Password == encPwd && vm.ID == u.ID).FirstOrDefault();
                if(user == null)
                {
                    ViewBag.ErrorMsg = "wrong password!!!";
                    return View(CurrentContext.GetCurUser());
                }
                if (list.Any(u => u.Email == vm.Email && u.ID != vm.ID))
                {
                    ViewBag.ErrorMsg = "this email has been already used ";
                    return View(CurrentContext.GetCurUser());
                }
                
            }

            int id = CurrentContext.GetCurUser().ID;
            using (var ctx = new AuctionSiteDBEntities())
            {
                var user = ctx.Users.Where(u => u.ID == vm.ID).FirstOrDefault<User>();
                user.Email = vm.Email;
                user.Name = vm.Name;
                ctx.SaveChanges();
                ViewBag.ErrorMsg = "Change info success!!!";
                return View(CurrentContext.GetCurUser());
            }
        }

        //Change Password
        [CheckLogin]
        public ActionResult ChangePassword()
        {
            int id = CurrentContext.GetCurUser().ID;
            using (var ctx = new AuctionSiteDBEntities())
            {
                var user = ctx.Users.Where(u => u.ID == id).FirstOrDefault();
                return View(user);
            }
        }

        //Change Pass
        [CheckLogin]
        [HttpPost]
        public ActionResult ChangePassword(User vm)
        {
            using (var dt = new AuctionSiteDBEntities())
            {
                string encPwd = StringUtils.Md5(vm.Password);
                var list = dt.Users.ToList();
                var user = dt.Users.Where(u => u.Password == encPwd && vm.ID == u.ID).FirstOrDefault();
                if (user == null)
                {
                    ViewBag.ErrorMsg = "Current password is wrong!!!";
                    return View(CurrentContext.GetCurUser());
                }
                if (vm.NewPassword != vm.NewPasswordAgain)
                {
                    ViewBag.ErrorMsg = "Check your new password!!!";
                    return View(CurrentContext.GetCurUser());
                } 
            }

            using (var ctx = new AuctionSiteDBEntities())
            {
                var user = ctx.Users.Where(u => u.ID == vm.ID).FirstOrDefault<User>();
                user.Password = StringUtils.Md5(vm.NewPassword);
                ctx.SaveChanges();
                ViewBag.ErrorMsg = "Change Password success!!!";
                return View(CurrentContext.GetCurUser());
            }
        }
    }
}