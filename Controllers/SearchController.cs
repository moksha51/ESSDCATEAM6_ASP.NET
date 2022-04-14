using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CATeam6.DB;
using CATeam6.Models;

namespace CATeam6.Controllers
{
    public class SearchController : Controller
    {
        private MyDBContext dbContext;
        public SearchController(MyDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index(string searchStr)
        {
            List<Products> allProducts = dbContext.Products.ToList();

            if (searchStr != null)
            {
                List<Products> products = dbContext.Products.Where(x =>
                x.ProductName.Contains(searchStr) ||
                x.ProductDescription.Contains(searchStr)).ToList();
                ViewData["AllProducts"] = products;
            }
            else
            {
                ViewData["AllProducts"] = allProducts;
                searchStr = "";
                return RedirectToAction("Index", "Home");

            }
            return View();
        }
    }
}
