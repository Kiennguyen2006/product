using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using nkkfirstdatabase.Models;

namespace nkkfirstdatabase.Controllers
{
    public class NkkembersController : Controller
    {
        private readonly NkkK24cnt2Context _context;

        public NkkembersController(NkkK24cnt2Context context)
        {
            _context = context;
        }

        // GET: Nkkembers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Nkkembers.ToListAsync());
        }

        // GET: Nkkembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nkkember = await _context.Nkkembers
                .FirstOrDefaultAsync(m => m.NkkMemberId == id);
            if (nkkember == null)
            {
                return NotFound();
            }

            return View(nkkember);
        }

        // GET: Nkkembers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Nkkembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NkkMemberId,Nkkuser,Nkkpassword,Nkkfullname,Nkkemail,Nkkphone")] Nkkember nkkember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nkkember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nkkember);
        }

        // GET: Nkkembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nkkember = await _context.Nkkembers.FindAsync(id);
            if (nkkember == null)
            {
                return NotFound();
            }
            return View(nkkember);
        }

        // POST: Nkkembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NkkMemberId,Nkkuser,Nkkpassword,Nkkfullname,Nkkemail,Nkkphone")] Nkkember nkkember)
        {
            if (id != nkkember.NkkMemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nkkember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NkkemberExists(nkkember.NkkMemberId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nkkember);
        }

        // GET: Nkkembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nkkember = await _context.Nkkembers
                .FirstOrDefaultAsync(m => m.NkkMemberId == id);
            if (nkkember == null)
            {
                return NotFound();
            }

            return View(nkkember);
        }

        // POST: Nkkembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nkkember = await _context.Nkkembers.FindAsync(id);
            if (nkkember != null)
            {
                _context.Nkkembers.Remove(nkkember);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NkkemberExists(int id)
        {
            return _context.Nkkembers.Any(e => e.NkkMemberId == id);
        }
    }
}
