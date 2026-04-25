using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TienIchTaiXe.Libraries.Models;

namespace TienIchTaiXe.Data.FuentAPI;

public class BlogConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        // Cấu hình tên bảng
        builder.ToTable("Blogs");

        // Cấu hình Khóa chính (Mặc định EF Core tự hiểu Id là khóa chính và tự tăng, 
        // nhưng viết rõ ra sẽ an toàn và dễ đọc hơn)
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        // Cấu hình các Property
        builder.Property(x => x.Title)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(x => x.Slug)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(x => x.Summary)
               .HasMaxLength(500);

        builder.Property(x => x.Content)
               .IsRequired();

        builder.Property(x => x.CoverImageUrl)
               .HasColumnType("nvarchar(max)"); // Chuyển đổi chuẩn xác TypeName

        // Cấu hình SEO và Tags
        builder.Property(x => x.MetaDescription)
               .HasMaxLength(160);

        builder.Property(x => x.Category)
               .HasMaxLength(100);

        builder.Property(x => x.Tags)
               .HasMaxLength(250);

        /* * CÁC TRƯỜNG HỢP CÒN LẠI (Author, CreatedAt, IsPublished, ViewCount...)
         * EF Core sẽ tự động map theo kiểu dữ liệu mặc định của C#.
         * Các gán giá trị mặc định (như ViewCount = 0) ở file Model sẽ hoạt động ở tầng ứng dụng (Application level).
         * Nếu bạn muốn ép giá trị mặc định sâu xuống tận Database (tạo constraint DEFAULT trong SQL), 
         * bạn có thể thêm như sau (không bắt buộc):
         * builder.Property(x => x.IsPublished).HasDefaultValue(true);
         * builder.Property(x => x.ViewCount).HasDefaultValue(0);
         */
    }
}  
