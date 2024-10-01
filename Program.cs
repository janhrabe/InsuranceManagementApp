using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InsuranceManagementApp.Data;

namespace InsuranceManagementApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Přidání služeb do kontejneru
        // Získání connection string pro hlavní databázi
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        // Získání connection string pro databázi pojištění
        var insuranceConnectionString = builder.Configuration.GetConnectionString("InsuranceConnection") ?? throw new InvalidOperationException("Connection string 'InsuranceConnection' not found.");
        builder.Services.AddDbContext<InsuranceDbContext>(options =>
            options.UseSqlite(insuranceConnectionString)); // Použití SQLite pro InsuranceDbContext

        // Získání connection string pro databázi kontaktů
        var contactConnectionString = builder.Configuration.GetConnectionString("ContactConnection") ?? throw new InvalidOperationException("Connection string 'ContactConnection' not found.");
        builder.Services.AddDbContext<ContactDbContext>(options =>
            options.UseSqlite(contactConnectionString)); // Použití SQLite pro ContactDbContext

        // Nastavení Identity s přizpůsobenými možnostmi
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 8; // Minimální délka hesla
            options.Password.RequireNonAlphanumeric = false; // Nevyžaduje speciální znaky v hesle
            options.User.RequireUniqueEmail = true; // Vyžaduje unikátní email pro uživatele
        })
        .AddEntityFrameworkStores<ApplicationDbContext>(); // Použití ApplicationDbContext pro Identity

        builder.Services.AddControllersWithViews(); // Přidání služeb pro MVC (model-view-controller)
        var app = builder.Build();

        // Konfigurace HTTP pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        // Mapování kontrolerů na konkrétní URL vzor
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // Inicializace rolí a uživatelů
        using (IServiceScope scope = app.Services.CreateScope())
        {
            RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            IdentityUser? defaultAdminUser = await userManager.FindByEmailAsync("admin@admin.cz");

            // Vytvoření role Admin, pokud neexistuje
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            // Přidání uživatele do role Admin, pokud ještě není
            if (defaultAdminUser is not null && !await userManager.IsInRoleAsync(defaultAdminUser, UserRoles.Admin))
                await userManager.AddToRoleAsync(defaultAdminUser, UserRoles.Admin);
        }

        app.Run();
    }
}

