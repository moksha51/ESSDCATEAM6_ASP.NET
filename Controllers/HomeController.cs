using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using CATeam6.Models;
using CATeam6.DB;

namespace CATeam6.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDBContext dBContext;
        private readonly IWebHostEnvironment env;

        public HomeController(IWebHostEnvironment env, MyDBContext dBContext)
        {
            this.env = env;
            this.dBContext = dBContext;
        }


        public IActionResult Index()
        {
            Session session = ValidateSession();
            // No cookie (Brand new user) case: Make cookies
            if (session == null) {
                Session newSession = new Session();
                dBContext.Add(newSession);
                dBContext.SaveChanges();
                string newSessionId = newSession.Id.ToString();
                Response.Cookies.Append("SessionId", newSessionId);
                session = newSession;
            }

            //TODO: Display cart status based on session
            DBUtility db = new DBUtility(dBContext);
            List<Products> allProducts = dBContext.Products.ToList(); //Show all products on the Homepage 


            //start of cartqty counter
            int totalqty = 0;
            List<Cart> currentCart = new List<Cart>();
            if (session.User == null)
            {
                currentCart = dBContext.Carts.Where(x => x.SessionId.Id == session.Id).ToList();
                foreach (Cart c in currentCart) {
                    totalqty = totalqty + c.Quantity;
                }
            }
            else if (session.User != null) {
                currentCart = dBContext.Carts.Where(x => x.UserId.Id == session.User.Id).ToList();
                foreach (Cart c in currentCart)
                {
                    totalqty = totalqty + c.Quantity;
                }
            }
            ViewData["totalQuantity"] = totalqty;
            //end of cartqty counter

            ViewData["AllProducts"] = allProducts;
            
            return View();
        }

        private Session ValidateSession()
        {
            //Cookies available (Returning user) case
            if (Request.Cookies["SessionId"] != null)
            {
                Guid sessionId = Guid.Parse(Request.Cookies["SessionId"]);
                Session session = dBContext.Sessions.FirstOrDefault(x =>
                    x.Id == sessionId
                );
                return session;
            }
            return null;
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
