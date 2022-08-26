using Microsoft.AspNetCore.Identity;
using etherapist.Data;
using Microsoft.EntityFrameworkCore;
using etherapist.Utility;
using etherapist.Models;

namespace etherapist.Data.DbInitializer;

public class DbInitializer: IDbInitializer {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _db;

    public DbInitializer (
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext db

    ) {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _roleManager = roleManager;
        _db = db;
    }

    public async void Initialize()
    {
        // Apply Migrations
        try {
            if (_db.Database.GetPendingMigrations().Count() > 0) {
                _db.Database.Migrate();
            }
        }
        catch(Exception ex) {

        }

        if (_db.Subscriptions.ToList().Count == 0) {
            // Create Subscription Plans
            _db.Subscriptions.Add(new Subscription {
                PlanName = "Single Plan",
                Price = 8000,
                Benefits = "Includes a single therapy session with one of our professionals",
                AvailableSessions = 1
            });
            _db.Subscriptions.Add(new Subscription {
                PlanName = "Monthly Plan",
                Price = 20000,
                Benefits = "Includes access to sessions 4 times in one month and recommendations on how to schedule sessions for best results",
                AvailableSessions = 4
            });
            _db.Subscriptions.Add(new Subscription {
                PlanName = "Premium Plan",
                Price = 50000,
                Benefits = "Includes access to sessions 4 times in one month and recommendations on how to schedule sessions for best results",
                AvailableSessions = 12
            });
            
            _db.SaveChanges();
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

                // Create Admin user
                var adminUser = CreateUser();
                String adminEmail = "etherapistAdmin@gmail.com";

                await _userStore.SetUserNameAsync(adminUser, adminEmail, CancellationToken.None);
                await _emailStore.SetEmailAsync(adminUser, adminEmail, CancellationToken.None);
                adminUser.FirstName = "Admin";
                adminUser.LastName = "Admin";
                adminUser.EmailConfirmed = true;

                var adminUserResult = _userManager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();

                // Apply role
                ApplicationUser? adminUserCheck = _db.ApplicationUsers.FirstOrDefault(
                    user => user.Email == "etherapistAdmin@gmail.com"
                );
                _userManager.AddToRoleAsync(adminUserCheck, SD.Role_Admin).GetAwaiter().GetResult();

        }

        ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }


        return;
    }
    IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)_userStore;
    }
}