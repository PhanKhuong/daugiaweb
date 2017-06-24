using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using AuctionWeb.Helpers;
using System.Configuration;
using System.Threading.Tasks;

namespace AuctionWeb.Controllers
{
    public class MailingController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress("ptkhuong96@gmail.com"));
            message.Subject = "announcement from HIT AUCTION ";
            message.Body = "<table style=\"width:300px\"><tr><td>Jill</td><td>Smith</td> <td>50</td></tr><tr><td>Eve</td><td>Jackson</td><td>94</td></tr></table>";
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                await smtp.SendMailAsync(message);
                return new EmptyResult();
            }
        }
    }
}