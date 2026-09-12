using Microsoft.AspNetCore.Mvc;
using nkklession07.Models.DataModel;

namespace nkklession07.Controllers
{
    public class nkkMemberController : Controller
    {
        public IActionResult Index()
        {
            return View(_members);
        }
        public IActionResult GetMember()
        {
            var member = new nkkMember
            {
                nkkMemberId = Guid.NewGuid().ToString(),
                nkkMemberName = "kien",
                nkkMemberpassword = "password123",
                nkkFullname = "Nguyen Khac Kien",
                nkkEmail = "nguyenkhackien@gmail.com"
            };
            //ViewBag.Member = member; 
            return View(member);
        }
        protected static List<nkkMember> _members = new List<nkkMember>
        {
                new nkkMember
                {
                    nkkMemberId = Guid.NewGuid().ToString(),
                    nkkMemberName = "kien",
                    nkkMemberpassword = "password123",
                    nkkFullname = "Nguyễn Khắc Kiên",
                    nkkEmail = "nguyenkhackien@gmail.com"
                },
                new nkkMember
                {
                    nkkMemberId = Guid.NewGuid().ToString(),
                    nkkMemberName = "nam2026",
                    nkkMemberpassword = "password456",
                    nkkFullname = "Trần Văn Nam",
                    nkkEmail = "namtv@gmail.com"
                },
                new nkkMember
                {
                    nkkMemberId = Guid.NewGuid().ToString(),
                    nkkMemberName = "hoa_dev",
                    nkkMemberpassword = "password789",
                    nkkFullname = "Lê Thị Hoa",
                    nkkEmail = "hoalt@gmail.com"
                }
            };
        public IActionResult GetMembers()
        {

            ViewBag.memberList = _members;
            return View();
        }
        //Get:Create memmber
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(nkkMember member)
        {
            if (ModelState.IsValid)
            {
                member.nkkMemberId = Guid.NewGuid().ToString();
                _members.Add(member);
                return RedirectToAction("GetMembers");
            }
            return View(member);

        }

    }
}
