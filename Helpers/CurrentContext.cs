using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuctionWeb.Models;

namespace AuctionWeb.Helpers
{
    public class CurrentContext
    {
        public static bool Islogged()
        {
            var flag = HttpContext.Current.Session["isLogin"];
            if(flag == null || (int)flag == 0)
            {
                if(HttpContext.Current.Request.Cookies["userId"] != null)
                {
                    int userIdCookie = Convert.ToInt32(HttpContext.Current.Request.Cookies["userId"].Value);
                    using (var ctx = new AuctionSiteDBEntities())
                    {
                        var user = ctx.Users.Where(u => u.ID == userIdCookie)
                        .FirstOrDefault();
                        HttpContext.Current.Session["isLogin"] = 1;
                        HttpContext.Current.Session["isLogin"] = user;
                    }
                   
                    return true;
                }

                return false;
            }
            return true;
        }

        public static User GetCurUser()
        {
            return (User)HttpContext.Current.Session["user"];
        }

        public static void Destroy()
        {
            HttpContext.Current.Session["isLogin"] = 0;
            HttpContext.Current.Session["user"] = null;

        }
    }
}