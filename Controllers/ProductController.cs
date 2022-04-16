using Microsoft.AspNetCore.Mvc;

namespace CATeam6.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
