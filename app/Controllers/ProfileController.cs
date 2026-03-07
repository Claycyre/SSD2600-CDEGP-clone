using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Data;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Controllers;

public class ProfileController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProfileController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ===============================
    // PROFILE INDEX
    // ===============================
    public IActionResult Index()
    {
        return View();
    }

    // ==========================================
    // CONTACT CONTROLLER WITH CRUD (MOVED HERE)
    // ==========================================

    public async Task<IActionResult> ContactIndex()
    {
        return View(await _context.ContactDetail.ToListAsync());
    }

    public async Task<IActionResult> ContactDetails(int? id)
    {
        if (id == null)
            return NotFound();

        var contact = await _context.ContactDetail.FirstOrDefaultAsync(m => m.PkContactId == id);

        if (contact == null)
            return NotFound();

        return View(contact);
    }

    public IActionResult ContactCreate()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ContactCreate(ContactDetail contact)
    {
        if (ModelState.IsValid)
        {
            _context.Add(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ContactIndex));
        }
        return View(contact);
    }

    public async Task<IActionResult> ContactEdit(int? id)
    {
        if (id == null)
            return NotFound();

        var contact = await _context.ContactDetail.FindAsync(id);
        if (contact == null)
            return NotFound();

        return View(contact);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ContactEdit(int id, ContactDetail contact)
    {
        if (id != contact.PkContactId)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(contact);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ContactDetail.Any(e => e.PkContactId == id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(ContactIndex));
        }

        return View(contact);
    }
}
