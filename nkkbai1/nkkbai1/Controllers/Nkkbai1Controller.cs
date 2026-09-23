using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using nkkbai1.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace nkkbai1.Controllers
{
    public class Nkkbai1Controller : Controller
    {
        private static readonly List<Models.Nkkbai1Model> _Sanpham = new List<Models.Nkkbai1Model>
{
    new Models.Nkkbai1Model
    {
        masp = 1,
        tensp = "Laptop Dell XPS 13",
        gia = 28500000m,
        soluong = 10,
        loai = 1,
        anh = "dell-xps13.jpg.jpg"
    },
    new Models.Nkkbai1Model
    {
        masp = 2,
        tensp = "Điện thoại iPhone 15 Pro",
        gia = 27990000m,
        soluong = 15,
        loai = 2,
        anh = "iphone15.jpg"
    },
    new Models.Nkkbai1Model
    {
        masp = 3,
        tensp = "Chuột không dây Logitech MX Master 3S",
        gia = 2450000m,
        soluong = 30,
        loai = 3,
        anh = "logitech-mx3s.jpg"
    }
}; 
        public IActionResult Nkkbai1(string khoanggia,string loai)
        {
            if(khoanggia==null && loai == null)
            {
                ViewBag.Sanpham = _Sanpham;
            }
            else
            {
                // cách tìm kiếm 1
                if (khoanggia == "1")
                {
                    ViewBag.Sanpham = _Sanpham.Where(sp => sp.gia < 100000).ToList();
                }
                if (khoanggia == "2")
                {
                    ViewBag.Sanpham = _Sanpham.Where(sp => sp.gia >= 100000 && sp.gia <= 500000).ToList();
                }
                if (khoanggia == "3")
                {
                    ViewBag.Sanpham = _Sanpham.Where(sp => sp.gia > 500000 && sp.gia <= 1000000).ToList();
                }
                if (khoanggia == "4")
                {
                    ViewBag.Sanpham = _Sanpham.Where(sp => sp.gia > 1000000 && sp.gia <= 3000000).ToList();
                }
                if (khoanggia == "5")
                {
                    ViewBag.Sanpham = _Sanpham.Where(sp => sp.gia > 3000000).ToList();
                }
                if (loai != null)
                {
                    int idLoai = int.Parse(loai);
                    for (int i = 0; i < _Sanpham.Count; i++)
                    {
                        if (_Sanpham[i].loai == idLoai)
                        {
                            ViewBag.Sanpham = _Sanpham.Where(sp => sp.loai == int.Parse(loai)).ToList();
                        }
                    }
                }
                if (khoanggia == "1" && loai != null)
                {
                    foreach (var i in _Sanpham)
                    {
                        if (i.gia <= 100000 && i.loai == int.Parse(loai))
                        {
                            ViewBag.Sanpham = _Sanpham.Where(sp => sp.gia <= 100000 && sp.loai == int.Parse(loai)).ToList();
                        }
                    }
                }
                if (khoanggia == "2" && loai != null)
                {
                    foreach (var i in _Sanpham)
                    {
                        if (i.gia >= 100000 && i.gia <= 500000 && i.loai == int.Parse(loai))
                        {
                            ViewBag.Sanpham = _Sanpham.Where(sp => sp.gia >= 100000 && sp.gia <= 500000 && sp.loai == int.Parse(loai)).ToList();
                        }
                    }
                }
                if (khoanggia == "3" && loai != null)
                {
                    foreach (var i in _Sanpham)
                    {
                        if (i.gia >= 500000 && i.gia <= 1000000 && i.loai == int.Parse(loai))
                        {
                            ViewBag.Sanpham = _Sanpham.Where(sp => sp.gia >= 500000 && sp.gia <= 100000 && sp.loai == int.Parse(loai)).ToList();
                        }
                    }
                }
                if (khoanggia == "4" && loai != null)
                {
                    foreach (var i in _Sanpham)
                    {
                        if (i.gia >= 1000000 && i.loai == int.Parse(loai))
                        {
                            ViewBag.Sanpham = _Sanpham.Where(sp => sp.gia >= 1000000 && sp.gia <= 3000000 && sp.loai == int.Parse(loai)).ToList();
                        }
                    }
                }
                if (khoanggia == "5" && loai != null)
                {
                    foreach (var i in _Sanpham)
                    {
                        if (i.gia > 3000000 && i.loai == int.Parse(loai))
                        {
                            ViewBag.Sanpham = _Sanpham.Where(sp => sp.gia > 3000000 && sp.loai == int.Parse(loai)).ToList();
                        }
                    }
                }
                // cách tìm kiếm 2
                //string dk = "";
                //if(khoanggia != null)
                //{
                //    if (khoanggia == "1")
                //    {
                //        dk += "gia < 100000";
                //    }
                //    else if (khoanggia == "2")
                //    {
                //        dk += "gia >= 100000 && gia <= 500000";
                //    }
                //    else if (khoanggia == "3")
                //    {
                //        dk += "gia > 500000 && gia <= 1000000";
                //    }
                //    else if (khoanggia == "4")
                //    {
                //        dk += "gia > 1000000 && gia <= 3000000";
                //    }
                //    else if (khoanggia == "5")
                //    {
                //        dk += "gia > 3000000";
                //    }
                //    if(loai != null)
                //    {
                //        dk += " && loai == " + loai;
                //    }
                //}
            }
            return View();
        }
        public IActionResult Them()
        {
            return View();
        }
        private readonly IWebHostEnvironment _env;

        public Nkkbai1Controller(IWebHostEnvironment env)
        {
            _env = env;
        }
        [HttpPost]
        public async Task<IActionResult> Themsp(Nkkbai1Model sanpham, IFormFile anhFile)
        {
            if (anhFile != null && anhFile.Length > 0)
            {
                // Tạo tên file duy nhất (để tránh bị đè file nếu trùng tên)
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(anhFile.FileName);

                // Đường dẫn thư mục lưu ảnh: wwwroot/images
                string uploadDir = Path.Combine(_env.WebRootPath, "images");

                // Tạo thư mục "images" nếu chưa có
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // Đường dẫn đầy đủ của file
                string filePath = Path.Combine(uploadDir, fileName);

                // Copy dữ liệu file upload vào thư mục
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await anhFile.CopyToAsync(stream);
                }

                // 3. GÁN TÊN FILE VÀO BIẾN model.anh BẰNG TAY TẠI ĐÂY
                sanpham.anh = fileName;
            }
                sanpham.masp = _Sanpham.Any() ? _Sanpham.Max(x => x.masp) + 1 : 1;
                _Sanpham.Add(sanpham);
                return RedirectToAction("Nkkbai1");
        }
        public IActionResult Sua(int masua)
        {
            var a = _Sanpham.FirstOrDefault(sp => sp.masp == masua);
            return View(a);
        }
        [HttpPost]
        public async Task<IActionResult> Suasp(Nkkbai1Model sanpham, IFormFile anhFile)
        {
            if (anhFile != null && anhFile.Length > 0)
            {
                // Tạo tên file duy nhất (để tránh bị đè file nếu trùng tên)
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(anhFile.FileName);

                // Đường dẫn thư mục lưu ảnh: wwwroot/images
                string uploadDir = Path.Combine(_env.WebRootPath, "images");

                // Tạo thư mục "images" nếu chưa có
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // Đường dẫn đầy đủ của file
                string filePath = Path.Combine(uploadDir, fileName);

                // Copy dữ liệu file upload vào thư mục
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await anhFile.CopyToAsync(stream);
                }

                // ảnh đã đc gán vào biến trong model
                sanpham.anh = fileName;
            }
            for(int i = 0; i < _Sanpham.Count; i++)
            {
                if (_Sanpham[i].masp == sanpham.masp)
                {
                    _Sanpham[1].masp = sanpham.masp;
                    _Sanpham[i].tensp = sanpham.tensp;
                    _Sanpham[i].gia = sanpham.gia;
                    _Sanpham[i].soluong = sanpham.soluong;
                    _Sanpham[i].loai = sanpham.loai;
                    _Sanpham[i].anh =sanpham.anh;
                }
            }
            return RedirectToAction("Nkkbai1");
        }
        public IActionResult Xoa(int maxoa)
        {
            // cách xoá 1
            //var dong = _Sanpham.FirstOrDefault(sp => sp.masp == maxoa);
            //_Sanpham.Remove(dong);
            //cách 3
            for(int i = 0; i < _Sanpham.Count; i++)
            {
                if (_Sanpham[i].masp == maxoa)
                {
                    _Sanpham.Remove(_Sanpham[i]);
                }
            }
            return View();
        }
        // cách xoá 2
        [HttpPost]
        public IActionResult xoasp(Nkkbai1Model sanpham)
        {
            for(int i = 0; i < _Sanpham.Count; i++)
            {
                if (_Sanpham[i].masp == sanpham.masp)
                {
                    _Sanpham.RemoveAt(i);
                }
            }
                return RedirectToAction("Nkkbai1");
        }
        }
}
