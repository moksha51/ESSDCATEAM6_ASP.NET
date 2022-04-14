﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CATeam6.Models;
using CATeam6.DB;

namespace CATeam6.Controllers
{
    public class LogoutController : Controller
    {
        private readonly MyDBContext dBContext;
        public LogoutController(MyDBContext dBcontext)
        {
            this.dBContext = dBcontext;
        }


        public IActionResult Index()
        {
            // remove session from our database
            if (Request.Cookies["SessionId"] != null)
            {
                Guid sessionId = Guid.Parse(Request.Cookies["sessionId"]);
                Session session = dBContext.Sessions.FirstOrDefault(x => x.Id == sessionId);

                if (session != null)
                {
                    // delete session record from our database;
                    dBContext.Remove(session);

                    // commit to save changes
                    dBContext.SaveChanges();
                }
            }

            // ask client to remove these cookies so that
            // they won't be sent over next time
            Response.Cookies.Delete("SessionId");
            Response.Cookies.Delete("Username");

            return RedirectToAction("Index", "Login");
        }
    }
}
