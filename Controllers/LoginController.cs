﻿using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using CATeam6.Models;
using CATeam6.DB;
using System;
using System.Collections.Generic;

namespace CATeam6.Controllers
{
    public class LoginController : Controller
    {

        private MyDBContext dBContext;
        public LoginController(MyDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public IActionResult Index()
        {
            if (Request.Cookies["SessionId"] != null)
            {
                Guid sessionId = Guid.Parse(Request.Cookies["sessionId"]);
                Session session = dBContext.Sessions.FirstOrDefault(x =>
                    x.Id == sessionId
                );
                User user = session.User;

                if (session == null)
                {
                    // someone has used an invalid Session ID (to fool us?); 
                    // route to Logout controller
                    return RedirectToAction("Index", "Logout");
                }

                if (user == null) {
                    // allow login to link user and sessionId
                    return View();
                }

                // valid Session ID + corresponding user; route to Home page
                return RedirectToAction("Index", "Home");
            }

            // no Session ID; route to homepage to get cookies
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login(IFormCollection form)
        {
            string username = form["username"];
            string password = form["password"];

            HashAlgorithm sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(
                Encoding.UTF8.GetBytes(username + password));

            User user = dBContext.Users.FirstOrDefault(x =>
                x.Username == username &&
                x.PassHash == hash
            );

            if (user == null) //invalid user case
            {
                TempData["Warning"] = "Invalid User"; //Can use this to display message via js 
                return RedirectToAction("Index", "Login");
            }

            // create a new session and tag to user
            Session session = dBContext.Sessions.FirstOrDefault(x => x.Id == Guid.Parse(Request.Cookies["SessionId"]));
            session.User = user;
            dBContext.SaveChanges();

            // ask browser to save and send back these cookies next time
            Response.Cookies.Append("Username", user.Username);

            //Merge session cart and user cart if required
            List<Cart> sessionCart = dBContext.Carts.Where(x => x.SessionId.Id == session.Id).ToList();
            if (sessionCart.Count() != 0) {
                foreach (Cart c in sessionCart) {
                    c.UserId = user;
                }
                dBContext.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
