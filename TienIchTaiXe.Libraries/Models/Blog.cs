namespace TienIchTaiXe.Libraries.Models;

public class Blog
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string? Author { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsPublished { get; set; } = true;
    public int ViewCount { get; set; } = 0;
    public string? MetaDescription { get; set; }
    public string? Category { get; set; }
    public string? Tags { get; set; }
}
