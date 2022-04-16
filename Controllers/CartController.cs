using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CATeam6.DB;
using CATeam6.Models;

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
            if (session.User == null)
            {
                return RedirectToAction("Index", "Login");
            }

            Guid userid = session.User.Id;
            TempData["Alert"] = "Successfully CheckOut!";  //send this tempdata to orders/index if the user visit orders page via clicking checkout button
            CreateOrder orderMaker = new CreateOrder(userid, dbContext);
            orderMaker.MakeOrder();  //with passing the data to user/product/order/actcode tables, the his data in cartitem table will be deleted.
            return RedirectToAction("Index", "Orders");
        }

        //[Route("Cart")]
        public IActionResult ViewCart()
        {
            List<Cart> cartItems;

            Session session = GetSession();
            if (session == null)  //no session
            {
                ViewData["cart"] = new List<Cart>(); //shows an empty cart
                return View("Cart");
            }
            else
            {
                Guid userId = session.User.Id;
                if (userId != null)  //user has logged in; So show user's cart
                {
                    cartItems = dbContext.Carts.Where(x => x.UserId.Id == userId).ToList();
                    ViewData["cart"] = cartItems;
                    string userCartAmt = cartItems.Sum(x => x.Quantity * x.Product.UnitPrice).ToString("#,0.00");
                    ViewData["userCartAmt"] = userCartAmt;
                }
                else
                { //user has not logged in; So show session's cart
                    cartItems = dbContext.Carts.Where(x => x.SessionId.Id == session.Id).ToList();
                    ViewData["cart"] = cartItems;
                    string userCartAmt = cartItems.Sum(x => x.Quantity * x.Product.UnitPrice).ToString("#,0.00");
                    ViewData["userCartAmt"] = userCartAmt;
                }
            }
            return View("Cart");
        }

        public IActionResult Update([FromBody] UpdateCart values)
        {
            int newquantity;
            string userCartAmt;
            Cart cartItem;
            double amt;

            Session session = GetSession();
            if (session.User != null) //if user has already logged in, use user's cart
            {
                Guid userId = session.User.Id;
                newquantity = values.Quantity;

                cartItem = dbContext.Carts.FirstOrDefault(x => x.UserId.Id == userId && x.Product.ProductId == values.ProductId);

                if (cartItem != null)
                {
                    cartItem.Quantity = newquantity;
                }

                dbContext.SaveChanges();

                amt = dbContext.Carts.Where(x => x.UserId.Id == userId).Sum(x => x.Quantity * x.Product.UnitPrice);

                userCartAmt = Math.Round(amt, 2).ToString("#,0.00");
            }
            else
            { //user not logged in yet, so link cart to session instead
                Guid sessionId = session.Id;
                newquantity = values.Quantity;

                cartItem = dbContext.Carts.FirstOrDefault(x => x.SessionId.Id == sessionId && x.Product.ProductId == values.ProductId);

                if (cartItem != null)
                {
                    cartItem.Quantity = newquantity;
                }

                dbContext.SaveChanges();

                amt = dbContext.Carts.Where(x => x.SessionId.Id == sessionId).Sum(x => x.Quantity * x.Product.UnitPrice);

                userCartAmt = Math.Round(amt, 2).ToString("#,0.00");
            }

            return Json(new
            {
                status = "success",
                userCartAmt
            });


        }

        public IActionResult Remove([FromBody] RemoveCart item)
        {
            int productId = item.ProductId;
            Cart cartItem;
            double amt;
            string userCartAmt;

            Session session = GetSession();
            User user = session.User;

            if (user == null)
            {
                cartItem = dbContext.Carts.FirstOrDefault(x => x.SessionId.Id == session.Id && x.Product.ProductId == item.ProductId);
                dbContext.Remove(cartItem);
                dbContext.SaveChanges();
                amt = dbContext.Carts.Where(x => x.SessionId.Id == session.Id).Sum(x => x.Quantity * x.Product.UnitPrice);
                userCartAmt = Math.Round(amt, 2).ToString("#,0.00");
            }
            else
            {
                cartItem = dbContext.Carts.FirstOrDefault(x => x.UserId.Id == user.Id && x.Product.ProductId == item.ProductId);
                dbContext.Remove(cartItem);
                dbContext.SaveChanges();
                amt = dbContext.Carts.Where(x => x.UserId.Id == user.Id).Sum(x => x.Quantity * x.Product.UnitPrice);
                userCartAmt = Math.Round(amt, 2).ToString("#,0.00");
            }

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

        //public ActionResult Add(Mobiles mo)
        //{
        //    if (Session["cart"] == null)
        //    {
        //        List<Mobiles> li = new List<Mobiles>();
        //        li.Add(mo);
        //        Session["cart"] = li;
        //        ViewBag.cart = li.Count();
        //        Session["count"] = 1;
        //    }
        //    else
        //    {
        //        List<Mobiles> li = (List<Mobiles>)Session["cart"];
        //        li.Add(mo);
        //        Session["cart"] = li;
        //        ViewBag.cart = li.Count();
        //        Session["count"] = Convert.ToInt32(Session["count"]) + 1;

        //    }
        //    return RedirectToAction("Index", "Home");
        //}
    }
}
