using TienIchTaiXe.Libraries.Entities;
using TienIchTaiXe.Libraries.Models.GGSheets;

namespace TienIchTaiXe.Services.Interfaces;

public interface IAuthService
{
    Task<GGSUser> GetGGSUser(string msnv);
    Task<GGSUser> GGSLogin(GGSUserLoginDto model);
    Task<bool> UpdateTokenUser(string _token);
}
