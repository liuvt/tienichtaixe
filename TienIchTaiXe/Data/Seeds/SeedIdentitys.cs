using TienIchTaiXe.Libraries.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TienIchTaiXe.Data.Seeds;
public static class SeedIdentitys
{
    public static class RoleNames
    {
        public const string Owner = "Owner";
        public const string Administrator = "Administrator";
        public const string Manager = "Manager";
        public const string User = "User";
        public static readonly string[] All = { Owner, Administrator, Manager, User };
    }

    /// <summary>
    /// Seed Roles bằng EF Core HasData (chạy qua migration).
    /// Lưu ý: ConcurrencyStamp nên cố định để tránh EF nghĩ "data changed" mỗi lần migrate.
    /// </summary>
    public static void SeedIdentityRoles(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "1",
                Name = RoleNames.Owner,
                NormalizedName = RoleNames.Owner.ToUpperInvariant(),
                ConcurrencyStamp = "ROLE-OWNER-STATIC"
            },
            new IdentityRole
            {
                Id = "2",
                Name = RoleNames.Administrator,
                NormalizedName = RoleNames.Administrator.ToUpperInvariant(),
                ConcurrencyStamp = "ROLE-ADMIN-STATIC"
            },
            new IdentityRole
            {
                Id = "3",
                Name = RoleNames.Manager,
                NormalizedName = RoleNames.Manager.ToUpperInvariant(),
                ConcurrencyStamp = "ROLE-MANAGER-STATIC"
            },
            new IdentityRole
            {
                Id = "4",
                Name = RoleNames.User,
                NormalizedName = RoleNames.User.ToUpperInvariant(),
                ConcurrencyStamp = "ROLE-USER-STATIC"
            }
        );
    }

    public static async Task SeedIdentityAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        string[] roles = { RoleNames.Owner, RoleNames.Administrator, RoleNames.Manager, RoleNames.User };
        foreach (var r in roles)
            if (!await roleManager.RoleExistsAsync(r))
                await roleManager.CreateAsync(new IdentityRole(r));

        async Task EnsureUser(string id, string email, string password, string role, string first, string last)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new AppUser
                {
                    Id = id,
                    UserName = email,
                    Email = email,
                    FirstName = first,
                    LastName = last,
                    PhoneNumber = "0949492972",
                    LockoutEnabled = false
                };

                var create = await userManager.CreateAsync(user, password);
                if (!create.Succeeded)
                    throw new Exception(string.Join("; ", create.Errors.Select(e => e.Description)));
            }

            if (!await userManager.IsInRoleAsync(user, role))
                await userManager.AddToRoleAsync(user, role);
        }
        await EnsureUser("OWNER-2026", "owner@tvtteam.com", "Owner@123", RoleNames.Owner, "Mr.", RoleNames.Owner);
        await EnsureUser("ADMIN-2026", "administrator@tvtteam.com", "Admin@123", RoleNames.Administrator, "AD", RoleNames.Administrator);
        await EnsureUser("MANAGER-2026", "manager@tvtteam.com", "Manager@123", RoleNames.Manager, "MR", RoleNames.Manager);
        await EnsureUser("USER-2026", "user@tvtteam.com", "User@123", RoleNames.User, "NV", RoleNames.User);
    }
}
