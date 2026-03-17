using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Supplier> Suppliers { get; set; } = default!;
    public DbSet<Transaction> Transactions { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OrderLineItem> OrderLineItems { get; set; } = default!;

    public DbSet<Product> Products { get; set; } = default!;

    public DbSet<ContactDetail> ContactDetail { get; set; }

    public DbSet<AdminMessage> AdminMessages { get; set; } = default!;

    //I think this is probably fine to do
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // AdminMessage has two FKs to the same ApplicationUser table.
        // SQL Server disallows multiple cascade paths, so both must use Restrict.
        builder
            .Entity<AdminMessage>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.FkSenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Entity<AdminMessage>()
            .HasOne(m => m.Recipient)
            .WithMany()
            .HasForeignKey(m => m.FkRecipientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
