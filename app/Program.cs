using brevo_csharp.Client;
using DotNetEnv;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SSD2600_CDEGP.Data;
using SSD2600_CDEGP.Models;
using SSD2600_CDEGP.Options;
using SSD2600_CDEGP.Repositories;
using SSD2600_CDEGP.Services;
using SSD2600_CDEGP.Services.Interfaces;
using Tailwind;

namespace SSD2600_CDEGP;

public class Program
{
    public static void Main(string[] args)
    {
        // This is a safety check, but doesn't really harm anything
        //Env.Load(Path.Combine(AppContext.BaseDirectory, ".env"));
        Env.Load(); // Fallback to default

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
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 0;
            })
            .AddRoles<IdentityRole>()
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

        // Register, Bind, and Validate strongly typed Options class
        // This ensures [Required] is checked on launch and will fail fast if null
        builder
            .Services.AddOptions<BrevoOptions>()
            .Bind(builder.Configuration.GetSection(BrevoOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddTransient<IEmailService, EmailService>();

        var app = builder.Build();

        // Configure the Brevo SDK using the validated settings
        using (var scope = app.Services.CreateScope())
        {
            var brevoSettings = scope
                .ServiceProvider.GetRequiredService<IOptions<BrevoOptions>>()
                .Value;

            // Set the global SDK key
            Configuration.Default.ApiKey["api-key"] = brevoSettings.ApiKey;
        }

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
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                DbInitializer.Seed(context, userManager, roleManager, shouldReseed);
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
