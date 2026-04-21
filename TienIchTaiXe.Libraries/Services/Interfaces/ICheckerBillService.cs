using TienIchTaiXe.Libraries.Entities;

namespace TienIchTaiXe.Libraries.Services.Interfaces;

public interface ICheckerBillService
{
    Task<ShiftWorkDto> Get(string userId, string? date);
}
