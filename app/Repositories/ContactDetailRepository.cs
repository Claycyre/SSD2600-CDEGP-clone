using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Data;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Repositories;

public class ContactDetailRepository
{
    private readonly ApplicationDbContext _context;

    public ContactDetailRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ContactDetail?> GetByIdAsync(int? id)
    {
        return await _context.ContactDetail.FirstOrDefaultAsync(c => c.PkContactId == (id ?? -1));
    }

    public async Task<ContactDetail?> GetByIdAsync(int? id, bool returnEmpty)
    {
        var contactDetail = await GetByIdAsync(id);
        if (contactDetail == null && returnEmpty)
        {
            return new ContactDetail();
        }

        return contactDetail;
    }

    public async Task<ContactDetail?> GetByUserAsync(ApplicationUser user)
    {
        return await GetByIdAsync(user.FkContactId);
    }

    public async Task<ContactDetail?> GetByUserAsync(ApplicationUser user, bool returnEmpty)
    {
        var contactDetail = await GetByUserAsync(user);
        if (contactDetail == null && returnEmpty)
        {
            return new ContactDetail();
        }

        return contactDetail;
    }

    public async Task<IEnumerable<ContactDetail>> GetAllAsync()
    {
        return await _context
            .ContactDetail.OrderBy(c => c.NameLast)
            .ThenBy(c => c.NameFirst)
            .ToListAsync();
    }

    public async Task AddAsync(ContactDetail contact)
    {
        await _context.ContactDetail.AddAsync(contact);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ContactDetail contact)
    {
        _context.ContactDetail.Update(contact);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var contact = await _context.ContactDetail.FindAsync(id);

        if (contact != null)
        {
            _context.ContactDetail.Remove(contact);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.ContactDetail.AnyAsync(c => c.PkContactId == id);
    }
}
