using CATeam6.DB;
using CATeam6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CATeam6.Controllers
{
    public class OrdersController : Controller
    {
        private MyDBContext dbContext;

        public OrdersController(MyDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            //This method will check if user is logged in to access puchases page, if not: redirect to home/index
            string SessionId = HttpContext.Request.Cookies["SessionId"];
            if (SessionId == null) {
                return RedirectToAction("Index", "Home");
            }

            //Captures the current userId based on session information and retrieves list of orders
            Session session = dbContext.Sessions.FirstOrDefault(x => x.Id.Equals(SessionId));
            if (session == null) // dk if this is required
            {
                ViewData["Orders"] = null;
                return View(); //TODO: Returns view of no orders
            }
            User user = dbContext.Sessions.FirstOrDefault(x => x.Id == session.Id).User;
            List<Orders> orders = dbContext.Orders.Where(x => x.UserId.Equals(user.Username)).ToList();
            ViewData["Orders"] = orders;
            if (orders.Count == 0)
            {
                return View(); //TODO: Returns view of no orders
            }

            //Creates list of all order details based off list of orders
            List<List<OrderDetails>> orderDetails = new List<List<OrderDetails>>();
            foreach (Orders order in orders) orderDetails.Add(dbContext.OrderDetails.Where(x => x.OrdersId == order.Id).ToList());
            ViewData["OrderDetails"] = orderDetails;

            //Creates a reference list of products
            List<Products> products = dbContext.Products.ToList();
            ViewData["Products"] = products;

            //Creates a reference to user for use in View
            ViewData["User"] = session.User.Username;

            return View();
        }
    }
}
