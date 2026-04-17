using TienIchTaiXe.Libraries.Models;

namespace TienIchTaiXe.Services.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetAllAsync();
        Task<Blog?> GetByIdAsync(int id);
        Task<Blog?> GetBySlugAsync(string slug);
        Task<IEnumerable<Blog>> GetRelatedAsync(string? category, int excludeId, int take = 4);
        Task<IEnumerable<Blog>> GetMostPopularAsync(int take = 5);
        Task<Blog> CreateAsync(Blog blog);
        Task<bool> UpdateAsync(Blog blog);
        Task<bool> DeleteAsync(int id);
        Task IncrementViewCountAsync(int id);
    }
}
