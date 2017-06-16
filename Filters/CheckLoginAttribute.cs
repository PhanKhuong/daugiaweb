using AuctionWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuctionWeb.Filters
{
    public class CheckLoginAttribute : ActionFilterAttribute
    {
        public int RequiredPermission { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(CurrentContext.Islogged() == false)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            if(CurrentContext.GetCurUser().Permission < this.RequiredPermission)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            base.OnActionExecuting(filterContext);
        }
    }
}