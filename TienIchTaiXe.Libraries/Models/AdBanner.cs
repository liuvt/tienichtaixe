using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TienIchTaiXe.Libraries.Models;

public enum AdPlacement
{
    HomeHeader = 1,          // Ví dụ: trên cùng trang chủ (đang dùng)
    HomeBelow = 2,     // Dưới ô tra cứu phạt nguội (đang dùng)
    Blogs = 3,     // Đầu trang danh sách bài viết (chưa dùng)
    BlogsBelow = 4,     // Giữa trang danh sách bài viết (chưa dùng)
    BlogsDetail = 5,     // Giữa ảnh tiêu đề và bài viết (đang dùng)
    BlogsDetailBelow = 6,     // Sau bài viết (chưa đùng)

    Sidebar = 7,
    Footer = 8,
    ArticleInline = 9,       // Giữa bài viết
    Custom = 99              // Tự định nghĩa nơi khác
}

[Table("AdBanners")]
public class AdBanner
{
    [Key]
    public int Id { get; set; }

    /// <summary>Tên nội bộ để quản lý, không nhất thiết hiển thị ra ngoài</summary>
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Tiêu đề / mô tả ngắn, có thể dùng làm alt text</summary>
    [MaxLength(250)]
    public string? Title { get; set; }

    /// <summary>Đường dẫn ảnh banner (relative hoặc absolute)</summary>
    [Required, MaxLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>Link khi click vào banner</summary>
    [Required, MaxLength(500)]
    public string LinkUrl { get; set; } = string.Empty;

    /// <summary>Target, mặc định mở tab mới</summary>
    [MaxLength(20)]
    public string Target { get; set; } = "_blank";

    /// <summary>Vị trí hiển thị (header, dưới ô tra cứu, sidebar…)</summary>
    [Required]
    public AdPlacement Placement { get; set; } = AdPlacement.HomeHeader;

    /// <summary>Thứ tự hiển thị trong slider (nhỏ đứng trước)</summary>
    public int DisplayOrder { get; set; } = 0;

    /// <summary>Đang bật hay tắt quảng cáo</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Chỉ hiển thị từ thời gian này (null = không giới hạn)</summary>
    public DateTime? StartAt { get; set; }

    /// <summary>Chỉ hiển thị đến thời gian này (null = không giới hạn)</summary>
    public DateTime? EndAt { get; set; }

    /// <summary>Ghi chú nội bộ (UTM, campaign name…)</summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    /// <summary>Ngày tạo</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Ngày cập nhật cuối</summary>
    public DateTime? UpdatedAt { get; set; }
}