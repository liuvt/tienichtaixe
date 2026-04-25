using Microsoft.EntityFrameworkCore;
using TienIchTaiXe.Data;
using TienIchTaiXe.Libraries.Models;
using TienIchTaiXe.Services.Interfaces;

namespace TienIchTaiXe.Services
{
    public class BlogService : IBlogService
    {
        #region SQL Controctor
        private readonly IDbContextFactory<tienichtaixeDBContext> _factory;

        // Tiêm IDbContextFactory thay vì tiêm DbContext trực tiếp
        public BlogService(IDbContextFactory<tienichtaixeDBContext> factory)
        {
            _factory = factory;
        }
        #endregion

        public async Task<IEnumerable<Blog>> GetAllAsync()
        {
            // Tạo context dùng 1 lần rồi hủy
            using var _context = await _factory.CreateDbContextAsync();

            return await _context.Blogs
                .AsNoTracking() // Chỉ đọc, không cần theo dõi thay đổi
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<Blog?> GetByIdAsync(int id)
        {
            using var _context = await _factory.CreateDbContextAsync();
            // Đổi FindAsync thành FirstOrDefaultAsync để có thể kẹp thêm AsNoTracking
            return await _context.Blogs
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Blog?> GetBySlugAsync(string slug)
        {
            using var _context = await _factory.CreateDbContextAsync();
            return await _context.Blogs
                .AsNoTracking() // Thêm dòng này để tăng tốc độ load trang chi tiết bài viết
                .FirstOrDefaultAsync(b => b.Slug == slug && b.IsPublished);
        }

        public async Task<IEnumerable<Blog>> GetRelatedAsync(string? category, int excludeId, int take = 4)
        {
            // Tạo context dùng 1 lần rồi hủy
            using var _context = await _factory.CreateDbContextAsync();
            return await _context.Blogs
                .AsNoTracking()
                .Where(b => b.IsPublished && b.Category == category && b.Id != excludeId)
                .OrderByDescending(b => b.CreatedAt)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<Blog>> GetMostPopularAsync(int take = 5)
        {
            // Tạo context dùng 1 lần rồi hủy
            using var _context = await _factory.CreateDbContextAsync();
            return await _context.Blogs
                .AsNoTracking()
                .Where(b => b.IsPublished)
                .OrderByDescending(b => b.ViewCount)
                .Take(take)
                .ToListAsync();
        }

        public async Task<Blog> CreateAsync(Blog blog)
        {
            // Tạo context dùng 1 lần rồi hủy
            using var _context = await _factory.CreateDbContextAsync();
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
            return blog;
        }

        public async Task<bool> UpdateAsync(Blog blog)
        {
            // Tạo context dùng 1 lần rồi hủy
            using var _context = await _factory.CreateDbContextAsync();
            _context.Blogs.Update(blog);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // Tạo context dùng 1 lần rồi hủy
            using var _context = await _factory.CreateDbContextAsync();
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) return false;

            _context.Blogs.Remove(blog);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task IncrementViewCountAsync(int id)
        {
            // Tạo context dùng 1 lần rồi hủy
            using var _context = await _factory.CreateDbContextAsync();

            // Tối ưu hóa: Bắn trực tiếp câu lệnh UPDATE xuống SQL Server mà không cần SELECT dữ liệu lên
            await _context.Blogs
                .Where(b => b.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(b => b.ViewCount, b => b.ViewCount + 1));
        }
    }
}
