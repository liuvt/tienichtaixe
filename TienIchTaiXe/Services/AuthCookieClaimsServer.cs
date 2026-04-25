using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TienIchTaiXe.Libraries.Models;

namespace TienIchTaiXe.Services;


//Add thêm thông tin claims vào cookie khi đăng nhập (dùng trong Blazor Server)
public sealed class AuthCookieClaimsServer
: UserClaimsPrincipalFactory<AppUser, IdentityRole>
{
    public AuthCookieClaimsServer(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    { }
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
    {
        var id = await base.GenerateClaimsAsync(user);

        // Add custom claim for cookie principal
        var giveName = $"{user.FirstName} {user.LastName}".Trim();

        if (!string.IsNullOrWhiteSpace(giveName))
            id.AddClaim(new Claim("give_name", giveName));

        // optional: chuẩn hóa luôn ClaimTypes.GivenName
        if (!string.IsNullOrWhiteSpace(giveName))
            id.AddClaim(new Claim(ClaimTypes.GivenName, giveName));

        return id;
    }
}