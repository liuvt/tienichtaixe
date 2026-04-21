
using TienIchTaiXe.Libraries.Models.GGSheets;

namespace TienIchTaiXe.Services.Interfaces;
public interface ICheckerSalaryService
{
    Task<Salary> GetSalary(string userId);
    Task<List<SalaryDetails>> GetSalaryDetails(string userId);

}
