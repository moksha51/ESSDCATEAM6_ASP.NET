using CATeam6.DB;
using CATeam6.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CATeam6.Controllers
{
    public class ProductsSortController : Controller
    {
        private MyDBContext dbContext;
        public ProductsSortController(MyDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult PriceAsce()
        {
            List<Products> allProducts = dbContext.Products.ToList();
            List<Products> priceAsce = allProducts.OrderBy(x => x.UnitPrice).ToList();

            ViewData["AllProducts"] = allProducts;
            ViewData["priceAsce"] = priceAsce;

            return View();
        }

        public IActionResult PriceDesc()
        {
            List<Products> allProducts = dbContext.Products.ToList();
            List<Products> priceDesc = allProducts.OrderByDescending(x => x.UnitPrice).ToList();

            ViewData["AllProducts"] = allProducts;
            ViewData["priceDesc"] = priceDesc;

            return View();
        }

        public IActionResult NameAsce()//start with A
        {
            List<Products> allProducts = dbContext.Products.ToList();
            List<Products> nameAsce = allProducts.OrderBy(x => x.ProductName).ToList();

            ViewData["AllProducts"] = allProducts;
            ViewData["nameA"] = nameAsce;
            return View();
        }

        public IActionResult NameDesc()//start with z
        {
            List<Products> allProducts = dbContext.Products.ToList();
            List<Products> nameDesc = allProducts.OrderByDescending(x => x.ProductName).ToList();

            ViewData["AllProducts"] = allProducts;
            ViewData["nameZ"] = nameDesc;
            return View();
        }
    }
}