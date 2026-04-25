using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TienIchTaiXe.Libraries.Models;

namespace TienIchTaiXe.Data.FluentAPIs;

public class AdBannerConfiguration : IEntityTypeConfiguration<AdBanner>
{
    public void Configure(EntityTypeBuilder<AdBanner> builder)
    {
        // Cấu hình tên bảng
        builder.ToTable("AdBanners");

        // Cấu hình Khóa chính
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        // Cấu hình các Property chuỗi (String)
        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.Title)
               .HasMaxLength(250);

        builder.Property(x => x.ImageUrl)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(x => x.LinkUrl)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(x => x.Target)
               .HasMaxLength(20);

        // Enum AdPlacement sẽ được EF Core ngầm định map thành kiểu int (hoặc string nếu bạn cấu hình Conversion).
        // Đặt IsRequired để đảm bảo luôn có giá trị vị trí cho quảng cáo.
        builder.Property(x => x.Placement)
               .IsRequired();

        builder.Property(x => x.Notes)
               .HasMaxLength(500);

        // Các trường kiểu số (int, bool) và DateTime đã có mặc định an toàn
        // nên không cần phải khai báo thêm ở đây trừ khi bạn muốn map kiểu DB cụ thể (như date thay vì datetime2).
    }
}
