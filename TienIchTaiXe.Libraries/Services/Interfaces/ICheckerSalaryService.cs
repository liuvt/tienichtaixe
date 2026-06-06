using TienIchTaiXe.Libraries.Entities;

namespace TienIchTaiXe.Services.Interfaces;
public interface ICheckerSalaryService
{
    Task<CheckerSalaryDto> Get(string userId, string? date = null);
}
