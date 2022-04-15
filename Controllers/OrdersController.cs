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
            if (Request.Cookies["SessionId"] == null ) {
                return RedirectToAction("Index", "Home");
            }
            Session session = dbContext.Sessions.FirstOrDefault(x => x.Id == Guid.Parse(Request.Cookies["SessionId"]));
            User user = session.User;


            if (session.User == null) {
                return RedirectToAction("Index", "Login");
            }

            //Captures the current userId based on session information and retrieves list of orders
            List<Orders> orders = dbContext.Orders.Where(x => x.UserId == user.Id).ToList();

            if (orders.Count() == 0)
            {
                ViewData["Orders"] = null;
                return View(); //TODO: Returns view of no orders
            }

            ViewData["Orders"] = orders;


            //Creates list of all order details based off list of orders
            List<List<OrderDetails>> orderDetails = new List<List<OrderDetails>>();
            foreach (Orders order in orders) orderDetails.Add(dbContext.OrderDetails.Where(x => x.OrdersId == order.Id).ToList());
            ViewData["OrderDetails"] = orderDetails;

            //Creates a reference list of products
            List<Products> products = dbContext.Products.ToList();
            ViewData["Products"] = products;

            //Creates a reference to user for use in View
            ViewData["User"] = user;

            return View();
        }
    }
}
