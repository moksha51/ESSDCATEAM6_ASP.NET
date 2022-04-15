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
            if (session == null)
            {
                // no session; bring user to Login page
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        private Session ValidateSession()
        {
            // check if there is a SessionId cookie
            if (Request.Cookies["SessionId"] == null)
            {
                return null;
            }

            // convert into a Guid type (from a string type)
            Guid sessionId = Guid.Parse(Request.Cookies["SessionId"]);
            Session session = dBContext.Sessions.FirstOrDefault(x =>
                x.Id == sessionId
            );

            return session;
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
