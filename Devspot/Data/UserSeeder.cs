using Devspot.Constants;
using Microsoft.AspNetCore.Identity;

namespace Devspot.Data;

public static class UserSeeder
{
    public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        await CreateUserWithRole(userManager, "admin@devspot.com", "Admin123!", Roles.Admin);
        await CreateUserWithRole(
            userManager,
            "jobseeker@devspot.com",
            "JobSeeker123!",
            Roles.JobSeeker
        );
        await CreateUserWithRole(
            userManager,
            "employer@devspot.com",
            "Employer123!",
            Roles.Employer
        );
    }

    private static async Task CreateUserWithRole(
        UserManager<IdentityUser> userManager,
        string email,
        string password,
        string role
    )
    {
        if (await userManager.FindByEmailAsync(email) is null)
        {
            var user = new IdentityUser
            {
                Email = email,
                EmailConfirmed = true,
                UserName = email,
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception(
                    $"Failed to create user with email {user.Email}. Errors: {string.Join(",", result.Errors)}"
                );

            await userManager.AddToRoleAsync(user, role);
        }
    }
}
