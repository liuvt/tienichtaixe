using Microsoft.AspNetCore.Mvc;
using TienIchTaiXe.Libraries.Models;
using TienIchTaiXe.Services.Interfaces;

namespace TienIchTaiXe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdBannerController : ControllerBase
{
    private readonly IAdBannerService _adBannerService;

    public AdBannerController(IAdBannerService adBannerService)
    {
        _adBannerService = adBannerService;
    }

    /// <summary>
    /// Dùng cho admin: lấy tất cả banner (bất kể active hay không)
    /// GET: /api/AdBanner
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdBanner>>> GetAll()
    {
        var items = await _adBannerService.GetAllAsync();
        return Ok(items);
    }

    /// <summary>
    /// Lấy 1 banner theo Id
    /// GET: /api/AdBanner/5
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AdBanner>> GetById(int id)
    {
        var item = await _adBannerService.GetByIdAsync(id);
        if (item == null)
            return NotFound();

        return Ok(item);
    }

    /// <summary>
    /// Dùng cho client: lấy banner đang chạy theo vị trí
    /// GET: /api/AdBanner/placement/HomeBelowSearch
    /// </summary>
    [HttpGet("placement/{placement}")]
    public async Task<ActionResult<IEnumerable<AdBanner>>> GetByPlacement(AdPlacement placement)
    {
        var items = await _adBannerService.GetRunningByPlacementAsync(placement);
        return Ok(items);
    }

    /// <summary>
    /// Tạo banner mới (admin)
    /// POST: /api/AdBanner
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AdBanner>> Create([FromBody] AdBanner banner)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _adBannerService.CreateAsync(banner);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Cập nhật banner (admin)
    /// PUT: /api/AdBanner/5
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AdBanner banner)
    {
        if (id != banner.Id)
            return BadRequest("Id không khớp.");

        var ok = await _adBannerService.UpdateAsync(banner);
        if (!ok)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Xoá banner (admin)
    /// DELETE: /api/AdBanner/5
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _adBannerService.DeleteAsync(id);
        if (!ok)
            return NotFound();

        return NoContent();
    }
}
