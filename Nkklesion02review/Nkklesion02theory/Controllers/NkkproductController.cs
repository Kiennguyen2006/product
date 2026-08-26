using Microsoft.AspNetCore.Mvc;
using Nkklesion02theory.Models;

namespace Nkklesion02theory.Controllers
{
    public class NkkproductController : Controller
    {
        public IActionResult Nkkindex()
        {
            ViewBag.name = "Nguyễn Khắc Kiên";
            ViewData["productNKK"] = "laptop dell";
            TempData["UNI"] = "Trường đại học Nguyễn Trãi-NTU";
            return View();
        }
        public IActionResult GetProduct()
        {
            NkkProduct nkkProduct = new NkkProduct()
            {
                ProductId = "241090045",
                ProductName = "Nguyễn Khắc Kiên",
                YearRelease = 2006,
                Price = 1000
            };

            ViewData["product"] = nkkProduct;
            ViewBag.product = nkkProduct;
            return View("product");
        }
    }
}
