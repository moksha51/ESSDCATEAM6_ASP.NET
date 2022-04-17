using System.Linq;
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
                

                if (session == null)
                {
                    // someone has used an invalid Session ID (to fool us?); 
                    // route to Logout controller
                    return RedirectToAction("Index", "Logout");
                }

                if (session.User == null)
                {
                    // allow login to link user and sessionId
                    return View();
                }
                else {
                    // valid Session ID + corresponding user; route to Home page
                    return RedirectToAction("Index", "Home");
                }

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

            //tags user to session
            Session session = dBContext.Sessions.FirstOrDefault(x => x.Id == Guid.Parse(Request.Cookies["SessionId"]));
            session.User = user;
            dBContext.SaveChanges();

            // ask browser to save and send back these cookies next time
            Response.Cookies.Append("Username", user.Username);

            //Merge session cart and user cart if required
            List<Cart> sessionCart = dBContext.Carts.Where(x => x.SessionId.Id == session.Id).ToList();
            
            if (sessionCart.Count() > 0)
            {
                List<Cart> userCart = dBContext.Carts.Where(x => x.UserId.Id == user.Id).ToList();
                foreach (Cart c in userCart)
                {
                    if (dBContext.Carts.FirstOrDefault(x => x.Product.ProductId == c.Product.ProductId && x.SessionId.Id == session.Id && x.UserId == null) != null) //products in session cart matching exisitng products in usercart but not tagged to user
                    {
                        Cart tempCart = dBContext.Carts.FirstOrDefault(x => x.Product.ProductId == c.Product.ProductId && x.SessionId.Id == session.Id && x.UserId == null);
                        c.Quantity = c.Quantity + tempCart.Quantity;
                        dBContext.Remove(tempCart);
                    } 
                }
                foreach (Cart c in sessionCart)
                {
                    if (c.UserId == null) //brand new products in session and not tagged to user
                    {
                        c.UserId = user;
                    }
                }
                dBContext.SaveChanges();
            }
            
            return RedirectToAction("Index", "Home");
        }
    }
}
