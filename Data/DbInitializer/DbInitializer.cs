using Microsoft.AspNetCore.Identity;
using etherapist.Data;
using Microsoft.EntityFrameworkCore;
using etherapist.Utility;
using etherapist.Models;

namespace etherapist.Data.DbInitializer;

public class DbInitializer: IDbInitializer {
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _db;

    public DbInitializer (
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext db
    ) {
        _userManager = userManager;
        _roleManager = roleManager;
        _db = db;
    }

    public void Initialize()
    {
        // Apply Migrations
        try {
            if (_db.Database.GetPendingMigrations().Count() > 0) {
                _db.Database.Migrate();
            }

        }
        catch(Exception ex) {

        }

        // Create Roles
        if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult()) 
        {
            _roleManager
                .CreateAsync(new IdentityRole(SD.Role_Patient))
                .GetAwaiter()
                .GetResult();
            _roleManager
                .CreateAsync(new IdentityRole(SD.Role_Admin))
                .GetAwaiter()
                .GetResult();
            _roleManager
                .CreateAsync(new IdentityRole(SD.Role_Therapist))
                .GetAwaiter()
                .GetResult();

            _userManager.CreateAsync(new ApplicationUser {
                FirstName = "saheed",
                LastName = "lawanson",
                Email = "etherapistAdmin@gmail.com",
                PhoneNumber = "+2349084559069",
            }, "Admin123").GetAwaiter().GetResult();

            // Apply role
            ApplicationUser? adminUser = _db.ApplicationUsers.FirstOrDefault(
                user => user.Email == "etherapistAdmin@gmail.com"
            );
            _userManager.AddToRoleAsync(adminUser, SD.Role_Admin).GetAwaiter().GetResult();
        }

        return;
    }
}