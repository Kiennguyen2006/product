
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nguyenkhackien_exam.Models;

public class NkkemployeesController : Controller
{
    private readonly Nkk2410900045Context _context;

    public NkkemployeesController(Nkk2410900045Context context)
    {
        _context = context;
    }

    // GET: NKKEMPLOYEES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Nkkemployees.ToListAsync());
    }

    // GET: NKKEMPLOYEES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nkkemployee = await _context.Nkkemployees
            .FirstOrDefaultAsync(m => m.Id == id);
        if (nkkemployee == null)
        {
            return NotFound();
        }

        return View(nkkemployee);
    }

    // GET: NKKEMPLOYEES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: NKKEMPLOYEES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Nkkemployee nkkemployee)
    {

        if (ModelState.IsValid)
        {
            _context.Add(nkkemployee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(nkkemployee);
    }

    // GET: NKKEMPLOYEES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nkkemployee = await _context.Nkkemployees.FindAsync(id);
        if (nkkemployee == null)
        {
            return NotFound();
        }
        return View(nkkemployee);
    }

    // POST: NKKEMPLOYEES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nkkname,Nkkgender,NkkbirthDay,Nkkemail,Nkkphone,Nkkactive")] Nkkemployee nkkemployee)
    {
        if (id != nkkemployee.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(nkkemployee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NkkemployeeExists(nkkemployee.Id))
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
        return View(nkkemployee);
    }

    // GET: NKKEMPLOYEES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nkkemployee = await _context.Nkkemployees
            .FirstOrDefaultAsync(m => m.Id == id);
        if (nkkemployee == null)
        {
            return NotFound();
        }

        return View(nkkemployee);
    }

    // POST: NKKEMPLOYEES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var nkkemployee = await _context.Nkkemployees.FindAsync(id);
        if (nkkemployee != null)
        {
            _context.Nkkemployees.Remove(nkkemployee);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool NkkemployeeExists(int? id)
    {
        return _context.Nkkemployees.Any(e => e.Id == id);
    }
}
