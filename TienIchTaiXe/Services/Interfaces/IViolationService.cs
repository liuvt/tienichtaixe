using TienIchTaiXe.Libraries.Models;

namespace TienIchTaiXe.Services.Interfaces;

public interface IViolationService
{
    Task<List<Violation>> Gets(string licensePlate);
}
