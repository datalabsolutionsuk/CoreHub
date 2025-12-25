using CoreHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoreHub.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedDefaultUserAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Create roles if they don't exist
        string[] roleNames = { "SuperAdmin", "Admin", "Manager", "Practitioner", "User" };
        
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Create Super Admin user
        var superAdminEmail = "admin@corehub.com";
        var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);

        if (superAdminUser == null)
        {
            superAdminUser = new ApplicationUser
            {
                UserName = superAdminEmail,
                Email = superAdminEmail,
                EmailConfirmed = true,
                FirstName = "Super",
                LastName = "Admin"
            };

            var result = await userManager.CreateAsync(superAdminUser, "Admin@123");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
            }
        }
        else
        {
            // Ensure existing admin has SuperAdmin role
            var roles = await userManager.GetRolesAsync(superAdminUser);
            if (!roles.Contains("SuperAdmin"))
            {
                await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
            }
        }
    }

    public static async Task InitializeAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        try
        {
            // Ensure database is created (creates Identity tables too)
            var created = await context.Database.EnsureCreatedAsync();
            
            if (!created)
            {
                // Database already exists, just seed
                await SeedDefaultUserAsync(serviceProvider);
            }
            else
            {
                // Fresh database, give it a moment then seed
                await Task.Delay(500);
                await SeedDefaultUserAsync(serviceProvider);
            }
        }
        catch (Exception ex)
        {
            // Log but don't crash - admin can be created manually
            Console.WriteLine($"Database initialization note: {ex.Message}");
            Console.WriteLine("Super Admin can be created manually if needed.");
        }
    }
}
