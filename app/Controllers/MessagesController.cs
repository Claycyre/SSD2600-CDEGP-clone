using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Data;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Controllers;

[Authorize]
public class MessagesController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public MessagesController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    // GET: /Messages
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var messages = await _db
            .AdminMessages.AsNoTracking()
            .Include(m => m.Sender)
            .Where(m => m.FkRecipientId == userId)
            .OrderByDescending(m => m.SentAt)
            .ToListAsync();

        return View(messages);
    }

    // GET: /Messages/Read/{id}
    public async Task<IActionResult> Read(int id)
    {
        var userId = _userManager.GetUserId(User);
        var message = await _db
            .AdminMessages.Include(m => m.Sender)
            .FirstOrDefaultAsync(m => m.Id == id && m.FkRecipientId == userId);

        if (message == null)
            return NotFound();

        if (!message.IsRead)
        {
            message.IsRead = true;
            await _db.SaveChangesAsync();
        }

        return View(message);
    }
}
