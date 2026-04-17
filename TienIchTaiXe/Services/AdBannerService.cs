using Microsoft.EntityFrameworkCore;
using TienIchTaiXe.Data;
using TienIchTaiXe.Libraries.Models;
using TienIchTaiXe.Services.Interfaces;

namespace TienIchTaiXe.Services;

public class AdBannerService : IAdBannerService
{
    private readonly tienichtaixeDBContext _db;

    public AdBannerService(tienichtaixeDBContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<AdBanner>> GetAllAsync()
    {
        return await _db.AdBanners
            .AsNoTracking()
            .OrderBy(x => x.Placement)
            .ThenBy(x => x.DisplayOrder)
            .ToListAsync();
    }

    public async Task<AdBanner?> GetByIdAsync(int id)
    {
        return await _db.AdBanners
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<AdBanner>> GetRunningByPlacementAsync(AdPlacement placement)
    {
        var now = DateTime.UtcNow;

        return await _db.AdBanners
            .AsNoTracking()
            .Where(x => x.IsActive
                        && x.Placement == placement
                        && (x.StartAt == null || x.StartAt <= now)
                        && (x.EndAt == null || x.EndAt >= now))
            .OrderBy(x => x.DisplayOrder)
            .ThenByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<AdBanner> CreateAsync(AdBanner banner)
    {
        banner.CreatedAt = DateTime.UtcNow;

        _db.AdBanners.Add(banner);
        await _db.SaveChangesAsync();
        return banner;
    }

    public async Task<bool> UpdateAsync(AdBanner banner)
    {
        var existing = await _db.AdBanners.FindAsync(banner.Id);
        if (existing == null) return false;

        // map các field cần cho phép update
        existing.Name = banner.Name;
        existing.Title = banner.Title;
        existing.ImageUrl = banner.ImageUrl;
        existing.LinkUrl = banner.LinkUrl;
        existing.Target = banner.Target;
        existing.Placement = banner.Placement;
        existing.DisplayOrder = banner.DisplayOrder;
        existing.IsActive = banner.IsActive;
        existing.StartAt = banner.StartAt;
        existing.EndAt = banner.EndAt;
        existing.Notes = banner.Notes;
        existing.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _db.AdBanners.FindAsync(id);
        if (existing == null) return false;

        _db.AdBanners.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }
}