using Microsoft.AspNetCore.Mvc;
using nkklession08.Models;

namespace nkklession08.Controllers
{
    public class NkkMemberController : Controller
    {
        private static List<NkkMember> _member = new List<NkkMember>()
        {
             new NkkMember()
    {
        NkkMemberId = "1",
        NkkUserName = "nkk01",
        NkkPassword = "123456",
        NkkFullName = "Nguyen Van A",
        NkkEmail = "nguyenvana@gmail.com"
    },

    new NkkMember()
    {
        NkkMemberId = "2",
        NkkUserName = "nkk02",
        NkkPassword = "123456",
        NkkFullName = "Nguyen Van B",
        NkkEmail = "nguyen vanb@gmail.com"
    },

    new NkkMember()
    {
        NkkMemberId = "3",
        NkkUserName = "nkk03",
        NkkPassword = "123456",
        NkkFullName = "Nguyen Van C",
        NkkEmail = "nguyenvanc@gmail.com"
    }
    };
        public IActionResult NkkIndex()
        {
            return View(_member);
        }
        public IActionResult NkkCreate()
        {
            var member = new NkkMember();
            return View(member);
        }
        [HttpPost]
        public IActionResult NkkCreate(NkkMember member)
        {
            member.NkkMemberId = Guid.NewGuid().ToString();
            _member.Add(member);
            return RedirectToAction("NkkIndex");
        }
        private string ma = "";
        public IActionResult NkkSua(string id) {
            ma = id;
            var member = _member.Where(m => m.NkkMemberId == id).FirstOrDefault();
            return View(member);
        }

        [HttpPost]
        public IActionResult Suasanpham(NkkMember member)
        {
            foreach (var item in _member)
            {
               if(item.NkkMemberId == member.NkkMemberId)
                {
                    item.NkkUserName = member.NkkUserName;
                    item.NkkPassword = member.NkkPassword;
                    item.NkkFullName = member.NkkFullName;
                    item.NkkEmail = member.NkkEmail;
               }
            }
            return RedirectToAction("NkkIndex");
        }
        public IActionResult Nkkchitiet(string id)
        {
            var member = _member.Where(m => m.NkkMemberId == id).FirstOrDefault();
            return View(member);
        }
        public IActionResult Nkkxoa(string id)
        {
            var member = _member.Where(m => m.NkkMemberId == id).FirstOrDefault();
            _member.Remove(member);
            return RedirectToAction("NkkIndex");
        }
    }
}
