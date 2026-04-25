using TienIchTaiXe.Libraries.Entities;

namespace TienIchTaiXe.Libraries.Services.Interfaces;

public interface IAuthJwtService
{
    Task<string> LoginTokenAsync(AppLoginDTO dto);
    Task LogoutTokenAsync();
}
