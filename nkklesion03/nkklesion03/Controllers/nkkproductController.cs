using Microsoft.AspNetCore.Mvc;
using nkklesion03.Models;

namespace nkklesion03.Controllers
{
    [Route("/Danh-sach-san-pham")]
    public class nkkproductController : Controller
    {
        private readonly List<nkkproduct> _products= new()
            {
                new nkkproduct { nkkproductid = "P001", nkkproductname = "Laptop Dell XPS 13", nkkyearrelease = 2022, nkkprice = 25000000m },
                new nkkproduct { nkkproductid = "P002", nkkproductname = "iPhone 14 Pro Max", nkkyearrelease = 2022, nkkprice = 28500000m },
                new nkkproduct { nkkproductid = "P003", nkkproductname = "Samsung Galaxy S23", nkkyearrelease = 2023, nkkprice = 21000000m },
                new nkkproduct { nkkproductid = "P004", nkkproductname = "MacBook Air M2", nkkyearrelease = 2022, nkkprice = 26900000m },
                new nkkproduct { nkkproductid = "P005", nkkproductname = "Bàn phím cơ Keychron K2", nkkyearrelease = 2021, nkkprice = 1950000m },
                new nkkproduct { nkkproductid = "P006", nkkproductname = "Chuột Logitech MX Master 3S", nkkyearrelease = 2022, nkkprice = 2450000m },
                new nkkproduct { nkkproductid = "P007", nkkproductname = "Màn hình LG UltraGear 27 inch", nkkyearrelease = 2023, nkkprice = 8500000m },
                new nkkproduct { nkkproductid = "P008", nkkproductname = "Tai nghe Sony WH-1000XM5", nkkyearrelease = 2022, nkkprice = 7900000m },
                new nkkproduct { nkkproductid = "P009", nkkproductname = "Đồng hồ Apple Watch Series 9", nkkyearrelease = 2023, nkkprice = 10490000m },
                new nkkproduct { nkkproductid = "P100", nkkproductname = "Máy tính bảng iPad Air 5", nkkyearrelease = 2022, nkkprice = 14800000m }
            };
        public IActionResult Index()
        {
            return Json(_products);
        }
        // colection =>view
        [Route("/All")]
        public IActionResult nkkgetallproduct()
        {
            ViewData["products"] = _products;
            return View();
        }
    }
}
