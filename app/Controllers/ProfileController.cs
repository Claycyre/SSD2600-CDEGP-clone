using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SSD2600_CDEGP.Models;
using SSD2600_CDEGP.Repositories;

namespace SSD2600_CDEGP.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ContactDetailRepository _contactRepo;

    public ProfileController(
        UserManager<ApplicationUser> userManager,
        ContactDetailRepository contactRepo
    )
    {
        _userManager = userManager;
        _contactRepo = contactRepo;
    }

    private async Task<ApplicationUser?> GetAuthUserAsync()
    {
        return await _userManager.GetUserAsync(User);
    }

    private async Task<ContactDetail?> GetAuthUserContactDetailAsync()
    {
        var user = await GetAuthUserAsync();
        return await _contactRepo.GetByIdAsync(user?.FkContactId);
    }

    public async Task<IActionResult> Index()
    {
        var contact = await GetAuthUserContactDetailAsync();

        if (contact == null)
        {
            return RedirectToAction(nameof(ContactCreate));
        }

        return View(contact);
    }

    //Contact controller start
    public async Task<IActionResult> ContactIndex()
    {
        var contacts = await _contactRepo.GetAllAsync();
        return View(contacts);
    }

    public async Task<IActionResult> ContactDetails(int? id)
    {
        if (id == null)
            return NotFound();

        var contact = await _contactRepo.GetByIdAsync(id.Value);

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
        if (!ModelState.IsValid)
            return View(contact);

        await _contactRepo.AddAsync(contact);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ContactEdit(int? id)
    {
        if (id == null)
            return NotFound();

        var contact = await _contactRepo.GetByIdAsync(id.Value);

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

        if (!ModelState.IsValid)
            return View(contact);

        await _contactRepo.UpdateAsync(contact);

        // After editing, return to Profile dashboard
        return RedirectToAction(nameof(Index));
    }
}
