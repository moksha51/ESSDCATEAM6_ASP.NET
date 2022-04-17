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
        [Route("Cart")]
        public IActionResult ViewCart()
        {
            List<Cart> cartItems;

            Session session = GetSession();
            if (session == null)  //no session
            {
                ViewData["cart"] = new List<Cart>(); //shows an empty cart
                return View("Index");
            }
            else {
                 
                if (session.User != null)  //user has logged in; So show user's cart
                {
                    Guid userId = session.User.Id;
                    cartItems = dbContext.Carts.Where(x => x.UserId.Id == userId).ToList();
                    ViewData["cart"] = cartItems;
                    string userCartAmt = cartItems.Sum(x => x.Quantity * x.Product.UnitPrice).ToString("#,0.00");
                    ViewData["userCartAmt"] = userCartAmt;
                }
                else { //user has not logged in; So show session's cart
                    cartItems = dbContext.Carts.Where(x => x.SessionId.Id == session.Id).ToList();
                    ViewData["cart"] = cartItems;
                    string userCartAmt = cartItems.Sum(x => x.Quantity * x.Product.UnitPrice).ToString("#,0.00");
                    ViewData["userCartAmt"] = userCartAmt;
                }
            }
            return View("Index");
        }
        [HttpPost]
        public IActionResult Add(int id)
        {
            string userCartAmt;
            Cart cartItem;
            double amt;

            Session session = GetSession();
            if (session.User != null) //if user has already logged in, use user's cart
            {
                Guid userId = session.User.Id;
                cartItem = dbContext.Carts.FirstOrDefault(x => x.UserId.Id == userId && x.Product.ProductId == id);

                if (cartItem == null)
                {
                    Cart newCartItem = new Cart()
                    {
                        Product = dbContext.Products.FirstOrDefault(x => x.ProductId == id),
                        Quantity = 1,
                        SessionId = session,
                        UserId = session.User
                    };
                    dbContext.Add(newCartItem);
                    dbContext.SaveChanges();
                }
                else {
                    cartItem.Quantity++;
                    dbContext.SaveChanges();
                }

                amt = dbContext.Carts.Where(x => x.UserId.Id == userId).Sum(x => x.Quantity * x.Product.UnitPrice);
                userCartAmt = Math.Round(amt, 2).ToString("#,0.00");
            }
            else { //user not logged in yet, so link cart to session instead
                Guid sessionId = session.Id;
                cartItem = dbContext.Carts.FirstOrDefault(x => x.SessionId.Id == sessionId && x.Product.ProductId == id);

                if (cartItem == null)
                {
                    Cart newCartItem = new Cart()
                    {
                        Product = dbContext.Products.FirstOrDefault(x => x.ProductId == id),
                        Quantity = 1,
                        SessionId = session,
                        UserId = session.User
                    };
                    dbContext.Add(newCartItem);
                    dbContext.SaveChanges();
                }
                else
                {
                    cartItem.Quantity++;
                    dbContext.SaveChanges();
                }

                amt = dbContext.Carts.Where(x => x.SessionId.Id == sessionId).Sum(x => x.Quantity * x.Product.UnitPrice);
                userCartAmt = Math.Round(amt, 2).ToString("#,0.00");
            }

            return Json(new
            {
                status = "success",
                userCartAmt
            });


        }

        public IActionResult Subtract(int id)
        {
            int pid = id;
            Cart cartItem;

            Session session = GetSession();

            if (session.User == null)
            {
                cartItem = dbContext.Carts.FirstOrDefault(x => x.SessionId.Id == session.Id && x.Product.ProductId == pid);
                if (cartItem != null)
                {
                    cartItem.Quantity--;
                    if (cartItem.Quantity < 1)
                    {
                        dbContext.Remove(cartItem);
                    }
                    dbContext.SaveChanges();
                }
            }
            else
            {

                cartItem = dbContext.Carts.FirstOrDefault(x => x.UserId.Id == session.User.Id && x.Product.ProductId == pid);
                if (cartItem != null) {
                    cartItem.Quantity--;
                    if (cartItem.Quantity < 1)
                    {
                        dbContext.Remove(cartItem);
                    }
                    dbContext.SaveChanges();
                }                    
            }

            return Json(new
            {
                status = "success"
            });
        }

        public IActionResult Remove([FromBody] RemoveCart item)
        {
            int id = int.Parse(item.ProductId);
            Cart cartItem;

            Session session = GetSession();

            if (session.User == null)
            {
                cartItem = dbContext.Carts.FirstOrDefault(x => x.SessionId.Id == session.Id && x.Product.ProductId == id);
                dbContext.Remove(cartItem);
                dbContext.SaveChanges();
            }
            else {
                
                cartItem = dbContext.Carts.FirstOrDefault(x => x.UserId.Id == session.User.Id && x.Product.ProductId == id);
                dbContext.Remove(cartItem);
                dbContext.SaveChanges();
            }

            return Json(new
            {
                status = "success"
            });
        }
/*
        public IActionResult Update([FromBody] int id, int qty) 
        {
            string userCartAmt;
            Cart cartItem;
            double amt;

            Session session = GetSession();
            if (session.User != null) //if user has already logged in, use user's cart
            {
                Guid userId = session.User.Id;
                cartItem = dbContext.Carts.FirstOrDefault(x => x.UserId.Id == userId && x.Product.ProductId == id);
                cartItem.Quantity = qty;
                dbContext.SaveChanges();

                amt = dbContext.Carts.Where(x => x.UserId.Id == userId).Sum(x => x.Quantity * x.Product.UnitPrice);
                userCartAmt = Math.Round(amt, 2).ToString("#,0.00");
            }
            else
            { //user not logged in yet, so link cart to session instead
                Guid sessionId = session.Id;
                cartItem = dbContext.Carts.FirstOrDefault(x => x.SessionId.Id == sessionId && x.Product.ProductId == id);
                cartItem.Quantity = qty;
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
 */   
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
