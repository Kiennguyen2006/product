using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nkklession09Annotation.Models.DataModels;
using nkklession09Annotation.Models.DataViewModels;
namespace nkklession09Annotation.Controllers

{
    public class NkkMemberController : Controller
    {
        private static List<NkkMember> _nkkMembers = new List<NkkMember>();
        // GET: NkkMemberController
        public ActionResult Index()
        {
            return View();
        }

        // GET: NkkMemberController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NkkMemberController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NkkMemberController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NkkMemberController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NkkMemberController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NkkMemberController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NkkMemberController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
