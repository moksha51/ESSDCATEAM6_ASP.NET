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
            //TODO: Display cart status based on session
            DBUtility db = new DBUtility(dBContext);
            List<Products> allProducts = dBContext.Products.ToList(); //Show all products on the Homepage 
            ViewData["AllProducts"] = allProducts;
            return View();
        }

        private Session ValidateSession()
        {


            // No cookie (Brand new user) case: Make cookies
            if (Request.Cookies["SessionId"] == null)
            {
                Session newSession = new Session();
                string newSessionId = newSession.Id.ToString();
                Response.Cookies.Append("SessionId", newSessionId);
                dBContext.Add(newSession);
                dBContext.SaveChanges();
                return newSession;
            }

            //Cookies available (Returning user) case
            Guid sessionId = Guid.Parse(Request.Cookies["SessionId"]);
            Session session = dBContext.Sessions.FirstOrDefault(x =>
                x.Id == sessionId
            );

            return session;
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
