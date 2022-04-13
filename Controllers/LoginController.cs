using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using CATeam6.Models;
using CATeam6.DB;

namespace CATeam6.Controllers
{
    public class LoginController : Controller
    {
        private MyDBContext dbContext;

        public LoginController(MyDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            if (Request.Cookies["SessionId"] != null)
            {
                Guid sessionId = Guid.Parse(Request.Cookies["sessionId"]);
                Session session = dbContext.Sessions.FirstOrDefault(x =>
                    x.SessionId == sessionId
                );

                if (session == null)
                {
                    return RedirectToAction("Index", "Logout");
                }

                // valid Session ID; route to Home page
                return RedirectToAction("Index", "Home");
            }

            // no Session ID; show Login page
            return View();
        }

        public IActionResult Login(IFormCollection form)
        {
            string username = form["username"];
            string password = form["password"];

            HashAlgorithm sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(
                Encoding.UTF8.GetBytes(username + password));

            User user = dbContext.Users.FirstOrDefault(x =>
                x.Username == username &&
                x.PassHash == hash
            );

            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // create a new session and tag to user
            Session session = new Session()
            {
                UserId = user
            }; // no need to generate session ID 
            dbContext.Sessions.Add(session);
            dbContext.SaveChanges();

            // ask browser to save and send back these cookies next time
            Response.Cookies.Append("SessionId", session.SessionId.ToString());
            Response.Cookies.Append("Username", user.Username);

            return RedirectToAction("Index", "Home");
        }
    }
}
