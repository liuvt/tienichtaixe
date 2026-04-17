using Microsoft.EntityFrameworkCore;
using TienIchTaiXe.Data;
using TienIchTaiXe.Libraries.Models;
using TienIchTaiXe.Services.Interfaces;

namespace TienIchTaiXe.Services
{
    public class BlogService : IBlogService
    {
        #region SQL Controctor
        private readonly tienichtaixeDBContext _context;
        public BlogService(tienichtaixeDBContext context)
        {
            this._context = context;
        }
        #endregion

        public async Task<IEnumerable<Blog>> GetAllAsync()
        {
            return await _context.Blogs
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<Blog?> GetByIdAsync(int id)
        {
            return await _context.Blogs.FindAsync(id);
        }

        public async Task<Blog?> GetBySlugAsync(string slug)
        {
            return await _context.Blogs
                .FirstOrDefaultAsync(b => b.Slug == slug && b.IsPublished);
        }

        public async Task<IEnumerable<Blog>> GetRelatedAsync(string? category, int excludeId, int take = 4)
        {
            return await _context.Blogs
                .Where(b => b.IsPublished && b.Category == category && b.Id != excludeId)
                .OrderByDescending(b => b.CreatedAt)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<Blog>> GetMostPopularAsync(int take = 5)
        {
            return await _context.Blogs
                .Where(b => b.IsPublished)
                .OrderByDescending(b => b.ViewCount)
                .Take(take)
                .ToListAsync();
        }

        public async Task<Blog> CreateAsync(Blog blog)
        {
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
            return blog;
        }

        public async Task<bool> UpdateAsync(Blog blog)
        {
            _context.Blogs.Update(blog);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) return false;

            _context.Blogs.Remove(blog);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task IncrementViewCountAsync(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                blog.ViewCount++;
                await _context.SaveChangesAsync();
            }
        }
    }
}
