using Microsoft.AspNetCore.Mvc;

namespace Nkklession13layout.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Search(string keyword)
        {
            ViewData["keyword"] = keyword;
            return View();
        }
        public IActionResult Host()
        {
            return View();
        }
    }
}
