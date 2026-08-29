using Microsoft.AspNetCore.Mvc;
using nkklesion04.Models;

namespace nkklesion04.Controllers
{
    public class nkkaccountController : Controller
    {
        private static readonly List<nkkaccount> nkkaccounts = new List<nkkaccount>
        {
            new nkkaccount
            {
                id = 1,
                name = "Nguyễn Văn An",
                email = "an.nguyen@example.com",
                phone = "0901234567",
                avartar = "/images/images1.jpg",
                address = "123 Đường Lê Lợi, Phường Bến Nghé, Quận 1, TP. Hồ Chí Minh",
                bio = "192.168.1.10",
                gender = 1, 
                birthday = new DateTime(1995, 5, 15)
            },
            new nkkaccount
            {
                id = 2,
                name = "Trần Thị Bích",
                email = "bich.tran@example.com",
                phone = "0912345678",
                avartar = "/images/images2.jpg",
                address = "456 Đường Trần Phú, Phường Lộc Thọ, TP. Nha Trang, Khánh Hòa",
                bio = "10.0.0.15",
                gender = 0,
                birthday = new DateTime(1998, 10, 20)
            },
            new nkkaccount
            {
                id = 3,
                name = "Lê Hoàng Cường",
                email = "cuong.le@example.com",
                phone = "0923456789",
                avartar = "/images/images3.jpg",
                address = "789 Đường Nguyễn Văn Linh, Phường Tân Phong, Quận 7, TP. Hồ Chí Minh",
                bio = "172.16.0.5",
                gender = 1,
                birthday = new DateTime(1990, 3, 8)
            },
            new nkkaccount
            {
                id = 4,
                name = "Phạm Dung Nhi",
                email = "nhi.pham@example.com",
                phone = "0934567890",
                avartar = "/images/images4.jpg",
                address = "12 Đường Hoàng Diệu, Phường 2, TP. Đà Lạt, Lâm Đồng",
                bio = "192.168.1.25",
                gender = 0,
                birthday = new DateTime(2001, 12, 1)
            },
            new nkkaccount
            {
                id = 5,
                name = "Hoàng Minh Đức",
                email = "duc.hoang@example.com",
                phone = "0945678901",
                avartar = "/images/images4.jpg",
                address = "101 Đường Cầu Giấy, Phường Quan Hoa, Quận Cầu Giấy, Hà Nội",
                bio = "10.0.0.88",
                gender = 1,
                birthday = new DateTime(1988, 7, 22)
            }
        };

        public IActionResult nkkindex()
        {
            ViewBag.mangdulieu = nkkaccounts;
            return View();
        }
        [Route("ho-so-cua-toi/{ma?}", Name = "profile")]
        public IActionResult nkkprofile(int ma = 1)
        {
            var dong = nkkaccounts.FirstOrDefault(x => x.id == ma);

            if (dong == null)
            {
                dong = nkkaccounts.FirstOrDefault();
            }

            return View(dong);
        }
    }
}