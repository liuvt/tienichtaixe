using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using TienIchTaiXe.Services.Interfaces;

namespace TienIchTaiXe.Controllers;

// Các đường dẫn có thể đánh dấu google bot không được truy cập hoặc được truy cập
[AllowAnonymous]
[Route("robots.txt")]
[ApiController]
public class RobotsController : ControllerBase
{
    private readonly ILogger<RobotsController> logger;
    public RobotsController(ILogger<RobotsController> _logger)
    {
        this.logger = _logger;
    }

    [HttpGet]
    [ResponseCache(Duration = 86400)]
    public IActionResult Index()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var sb = new StringBuilder();

        sb.AppendLine("User-agent: *");
        sb.AppendLine("Allow: /");
        sb.AppendLine();

        sb.AppendLine("# Không cho phép bot truy cập các vùng nội bộ");
        sb.AppendLine("Disallow: /admin/");
        sb.AppendLine("Disallow: /api/");
        sb.AppendLine("Disallow: /_framework/");
        sb.AppendLine("Disallow: /swagger/");       // nếu có swagger
        sb.AppendLine();

        sb.AppendLine("# Khai báo sitemap chính cho website");
        sb.AppendLine($"Sitemap: {baseUrl}/sitemap.xml");

        return Content(sb.ToString(), "text/plain", Encoding.UTF8);
    }
}

// Sở đồ web cho Google
[AllowAnonymous]
[Route("sitemap.xml")]
[ApiController]
public class SitemapController : ControllerBase
{
    private readonly ILogger<SitemapController> logger;
    private readonly IBlogService _blogService;

    public SitemapController(ILogger<SitemapController> _logger, IBlogService blogService)
    {
        this.logger = _logger;
        this._blogService = blogService;
    }

    [HttpGet]
    [ResponseCache(Duration = 86400)]
    public async Task<IActionResult> Index()
    {
        var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        var staticUrls = new[]
        {
            new { loc = baseUrl,                 priority = "1.0", changefreq = "daily"  },
            new { loc = $"{baseUrl}/blogs",      priority = "0.9", changefreq = "daily"  },
            new { loc = $"{baseUrl}/about",      priority = "0.6", changefreq = "yearly" },
        };

        var sb = new StringBuilder();
        sb.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
        sb.AppendLine(@"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">");

        // Static pages
        foreach (var url in staticUrls)
        {
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{url.loc}</loc>");
            sb.AppendLine($"    <lastmod>{today}</lastmod>");
            sb.AppendLine($"    <changefreq>{url.changefreq}</changefreq>");
            sb.AppendLine($"    <priority>{url.priority}</priority>");
            sb.AppendLine("  </url>");
        }

        // Blog posts
        var posts = await _blogService.GetAllAsync();
        foreach (var post in posts)
        {
            var lastMod = (post.UpdatedAt == default ? post.CreatedAt : post.UpdatedAt);

            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{baseUrl}/blogs/{post.Slug}</loc>");
            sb.AppendLine($"    <lastmod>{lastMod:yyyy-MM-dd}</lastmod>");
            sb.AppendLine("    <changefreq>monthly</changefreq>");
            sb.AppendLine("    <priority>0.8</priority>");
            sb.AppendLine("  </url>");
        }

        sb.AppendLine("</urlset>");

        return Content(sb.ToString(), "application/xml", Encoding.UTF8);
    }
}

// file quản cáo ads.txt cho Google: google.com, pub-4772558027240339, DIRECT, f08c47fec0942fa0
[AllowAnonymous]
[Route("ads.txt")]
[ApiController]
public class AdsController : ControllerBase
{
    [HttpGet]
    [ResponseCache(Duration = 86400)]
    public IActionResult Index()
    {
        const string content = "google.com, pub-4772558027240339, DIRECT, f08c47fec0942fa0";
        return Content(content, "text/plain; charset=utf-8");
    }
}

