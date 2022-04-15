using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CATeam6.DB;
using CATeam6.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CATeam6.Controllers
{
    public class CartController : Controller
    {
        private MyDBContext dbContext;

        public CartController(MyDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CheckOut()
        {
            Session session = GetSession();
            if (session == null)
            {
                return RedirectToAction("Index", "Login");
            }
            Guid userid = session.Id;
            TempData["Alert"] = "Successfully CheckOut!";//send this tempdata to mypurchase/index if the user visit my purchase page via clicking checkout button
            CreateOrder orderMaker = new CreateOrder(userid, dbContext);
            orderMaker.MakeOrder();//with passing the data to user/product/order/actcode tables, the his data in cartitem table will be deleted.
            return RedirectToAction("Index", "MyPurchase");
        }

        [Route("Cart")]
        public IActionResult ViewCart()
        {
            Session session = GetSession();
            if (session == null)
            {
                //return RedirectToAction("Index", "Login");
                ViewData["cart"] = new List<Cart>();
                return View();
            }

            Guid userid = session.User.Id;

            List<Cart> cartItems = dbContext.Carts.Where(x => x.UserId.Id == userid).ToList();
            ViewData["cart"] = cartItems;

            string userCartAmt = cartItems.Sum(x => x.Quantity * x.Product.UnitPrice).ToString("#,0.00");

            ViewData["userCartAmt"] = userCartAmt;

            return View();


        }

        public IActionResult Update([FromBody] UpdateCart values)
        {
            string username = Request.Cookies["Username"];
            User user = dbContext.Users.FirstOrDefault(x => x.Username == username);
            Guid userid = user.Id;
            int newquantity;
            string userCartAmt;

            newquantity = values.Quantity;


            Cart cartItem = dbContext.Carts.FirstOrDefault(x => x.UserId.Id == userid && x.Product.ProductId == values.ProductId);

            if (cartItem != null)
            {
                cartItem.Quantity = newquantity;
            }

            dbContext.SaveChanges();

            double amt = dbContext.Carts.Where(x => x.UserId.Id == userid).Sum(x => x.Quantity * x.Product.UnitPrice);

            userCartAmt = Math.Round(amt, 2).ToString("#,0.00");

            return Json(new
            {
                status = "success",
                userCartAmt
            });


        }

        public IActionResult Remove([FromBody] RemoveCart item)
        {
            string username = Request.Cookies["Username"];
            User user = dbContext.Users.FirstOrDefault(x => x.Username == username);
            Guid userid = user.Id;
            int productId = item.ProductId;

            Cart cartItem = dbContext.Carts.FirstOrDefault(x => x.UserId.Id == userid && x.Product.ProductId == item.ProductId);

            dbContext.Remove(cartItem);

            dbContext.SaveChanges();

            double amt = dbContext.Carts.Where(x => x.UserId.Id == userid).Sum(x => x.Quantity * x.Product.UnitPrice);

            string userCartAmt = Math.Round(amt, 2).ToString("#,0.00");



            return Json(new
            {
                status = "success",
                userCartAmt
            });
        }

        private Session GetSession()
        {
            if (Request.Cookies["SessionId"] == null)
            {
                return null;
            }

            Guid sessionId = Guid.Parse(Request.Cookies["SessionId"]);
            Session session = dbContext.Sessions.FirstOrDefault(x =>
                x.Id == sessionId
            );

            return session;
        }
    }
}
