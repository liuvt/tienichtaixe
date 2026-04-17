
using TienIchTaiXe.Libraries.Models;

namespace TienIchTaiXe.Services.Interfaces;

public interface IAdBannerService
{
    /// <summary>Lấy tất cả banner (kể cả đang tắt)</summary>
    Task<IEnumerable<AdBanner>> GetAllAsync();

    /// <summary>Lấy 1 banner theo Id</summary>
    Task<AdBanner?> GetByIdAsync(int id);

    /// <summary>
    /// Lấy các banner đang chạy (IsActive + trong khoảng StartAt/EndAt)
    /// tại 1 vị trí cụ thể (Placement)
    /// </summary>
    Task<IEnumerable<AdBanner>> GetRunningByPlacementAsync(AdPlacement placement);

    /// <summary>Tạo banner mới</summary>
    Task<AdBanner> CreateAsync(AdBanner banner);

    /// <summary>Cập nhật banner</summary>
    Task<bool> UpdateAsync(AdBanner banner);

    /// <summary>Xóa banner</summary>
    Task<bool> DeleteAsync(int id);
}
