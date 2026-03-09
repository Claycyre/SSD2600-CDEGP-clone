using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Data;
using SSD2600_CDEGP.Models;
using SSD2600_CDEGP.Repositories;
using SSD2600_CDEGP.Services;
using Tailwind;

namespace SSD2600_CDEGP;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString =
            builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found."
            );
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString)
        );
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddScoped<ContactDetailRepository>();

        //REPO INJECTION
        builder.Services.AddScoped<SupplierRepository>();
        builder.Services.AddScoped<ProductRepository>();

        builder
            .Services.AddDefaultIdentity<ApplicationUser>(options =>
                options.SignIn.RequireConfirmedAccount = true
            )
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();

        if (!builder.Environment.IsDevelopment())
        {
            builder
                .Services.AddDataProtection()
                .SetApplicationName("ssd2600")
                .PersistKeysToFileSystem(new DirectoryInfo(@"/var/dpkeys/"));
        }

        builder.UseTailwindCli();

        //Implement Singleton service for periodic table data
        builder.Services.AddSingleton<ElementService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();

            // DB Seeding in development
            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var shouldReseed =
                    args.Contains("--reseed") || app.Configuration.GetValue<bool>("RESEED_DB");
                var context = services.GetRequiredService<ApplicationDbContext>();
                DbInitializer.Seed(context, shouldReseed);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }

            if (args.Contains("--reseed"))
            {
                // This application was bootstrapped to perform a reseed on the DB.
                // Exit the application as this was probably all that was asked for.
                logger.LogInformation("Finished (re)seeding the DB. Exiting...");
                return;
            }
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "Identity",
            pattern: "{area:exists}/{controller=Account}/{action=Index}/{id?}"
        );

        app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}
