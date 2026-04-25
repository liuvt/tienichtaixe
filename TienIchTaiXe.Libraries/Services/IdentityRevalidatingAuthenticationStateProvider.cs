using TienIchTaiXe.Libraries.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TienIchTaiXe.Libraries.Services;
public sealed class IdentityRevalidatingAuthenticationStateProvider
    : RevalidatingServerAuthenticationStateProvider
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IOptions<IdentityOptions> _identityOptions;

    public IdentityRevalidatingAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory,
        IOptions<IdentityOptions> identityOptions)
        : base(loggerFactory)
    {
        _scopeFactory = scopeFactory;
        _identityOptions = identityOptions;
    }

    // Đây là khoảng thời gian giữa các lần gọi ValidateAuthenticationStateAsync để kiểm tra lại trạng thái xác thực của người dùng
    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

    // Validate the authentication state
    protected override async Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState authenticationState,
        CancellationToken cancellationToken)
    {
        // Kiểm tra xem user đã xác thực hay chưa
        var principal = authenticationState.User;
        if (principal.Identity?.IsAuthenticated != true)
            return false;

        // Kiểm tra xem userId trong Claims có tồn tại và hợp lệ không
        using var scope = _scopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var userId = userManager.GetUserId(principal);
        if (string.IsNullOrWhiteSpace(userId))
            return false;

        // Kiểm tra xem người dùng có tồn tại trong database không
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return false;

        // Kiểm tra SecurityStamp để đảm bảo người dùng vẫn hợp lệ (nếu UserManager hỗ trợ)
        if (userManager.SupportsUserSecurityStamp)
        {
            var stampType = _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType;
            var principalStamp = principal.FindFirstValue(stampType);
            var dbStamp = await userManager.GetSecurityStampAsync(user);
            return principalStamp == dbStamp;
        }

        return true;
    }
}
