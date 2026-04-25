using TienIchTaiXe.Libraries.Models;

namespace TienIchTaiXe.Libraries.Services.Interfaces;

public interface IAuthCookieService
{
    Task<AppUser?> GetMeAsync();
}