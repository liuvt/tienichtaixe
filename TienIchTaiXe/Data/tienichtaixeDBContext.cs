using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TienIchTaiXe.Libraries.Models;

namespace TienIchTaiXe.Data;

public partial class tienichtaixeDBContext : IdentityDbContext<AppUser>
{
    //Get config in appsetting
    private readonly IConfiguration configuration;

    public tienichtaixeDBContext(DbContextOptions<tienichtaixeDBContext> options, IConfiguration _configuration) : base(options)
    {
        //Models - Etityties
        configuration = _configuration;
    }

    //Call Model to create table in database
    public DbSet<AdBanner> AdBanners => Set<AdBanner>();
    public DbSet<Blog> Blogs => Set<Blog>();

    //Data seeding
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(tienichtaixeDBContext).Assembly);

        // Chạy ở program.cs hoặc qua migration
        //modelBuilder.SeedIdentityRoles();
    }

}

//Update tool: dotnet tool update --global dotnet-ef

//Create mirations: dotnet ef migrations add Init -o Data/Migrations
//Create database: dotnet ef database update


/* 
 * ///Publish project: 
dotnet publish -c Release --output ./Publish-TIENICHTAIXE TienIchTaiXe.csproj

 * ///Tailwind project: 
npx @tailwindcss/cli -i ./TienIchTaiXe/TailwindImport/input.css -o ./TienIchTaiXe/wwwroot/css/tailwindcss.css --watch
*/


