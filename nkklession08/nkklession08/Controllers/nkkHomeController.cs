using Microsoft.AspNetCore.Mvc;
using nkklession08.Models;
using System.Diagnostics;

namespace nkklession08.Controllers
{
    public class NkkHomeController : Controller
    {
        public IActionResult NkkIndex()
        {
            return View();
        }

        public IActionResult NkkPrivacy()
        {
            return View();
        }
        public IActionResult NkkAbout() {
            return View();    
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
