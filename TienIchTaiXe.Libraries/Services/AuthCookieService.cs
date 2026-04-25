using TienIchTaiXe.Libraries.Models;
using TienIchTaiXe.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using TienIchTaiXe.Libraries.Services.Interfaces;

namespace TienIchTaiXe.Libraries.Services;
public class AuthCookieService : IAuthCookieService
{
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly UserManager<AppUser> _userManager;

    // Lưu ý: AuthenticationStateProvider chỉ có thể lấy thông tin người dùng đã xác thực (đăng nhập) trong Blazor Server. Nếu chưa đăng nhập, GetMeAsync sẽ trả về null.
    public AuthCookieService(AuthenticationStateProvider authStateProvider, UserManager<AppUser> userManager)
    {
        _authStateProvider = authStateProvider;
        _userManager = userManager;
    }

    // Lấy thông tin người dùng hiện tại từ cookie (dùng trong Blazor Server)
    public async Task<AppUser?> GetMeAsync()
    {
        var state = await _authStateProvider.GetAuthenticationStateAsync();
        var user = state.User;
        if (user.Identity?.IsAuthenticated != true) return null;

        var userId = _userManager.GetUserId(user);
        if (string.IsNullOrWhiteSpace(userId)) return null;

        return await _userManager.FindByIdAsync(userId);
    }
}