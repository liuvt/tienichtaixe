using TienIchTaiXe.Libraries.Entities;
using TienIchTaiXe.Libraries.Models;
using Microsoft.AspNetCore.Identity;

namespace TienIchTaiXe.Services.Interfaces;

public interface IAuthServer
{
    // Cookie (web nội bộ)
    Task<AppUser> LoginCookie(AppLoginDTO login);
    Task LogoutCookie();

    // JWT (SPA/Mobile/Postman)
    Task<string> LoginJwt(AppLoginDTO login);

    // Account
    Task<IdentityResult> RegisterAsync(AppRegisterDTO register, CancellationToken ct = default);

    Task<AccountProfileDto> GetProfileAsync(string userId, CancellationToken ct = default);
    Task UpdateProfileAsync(string userId, AccountProfileUpdateDto dto, CancellationToken ct = default);
    Task ChangePasswordAsync(string userId, ChangePasswordDto dto, CancellationToken ct = default);

}