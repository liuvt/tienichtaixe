using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TienIchTaiXe.Libraries.Entities;
using TienIchTaiXe.Libraries.Models;
using TienIchTaiXe.Services.Interfaces;

namespace TienIchTaiXe.Controllers;
/* Đặt trên mỗi Method nếu muốn phân quyền riêng cho từng endpoint, hoặc đặt trên Controller nếu muốn áp dụng chung cho tất cả endpoint trong controller
        // 1) Cookie-only (web nội bộ)
        [Authorize(AuthenticationSchemes = "Identity.Application")]

        // 2) JWT-only (Postman/Mobile)
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        // 3) Cookie OR JWT (dùng policy)
        [Authorize(Policy = "CookieOrJwt")]
     */
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthServer _auth;
    private readonly UserManager<AppUser> _users;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthServer auth, UserManager<AppUser> users, ILogger<AuthController> logger)
    {
        _auth = auth;
        _users = users;
        _logger = logger;
    }

    // ===================== COOKIE =====================
    // Gọi bằng FORM từ browser: application/x-www-form-urlencoded
    // Sử dụng với mục đích đăng nhập web nội bộ
    [HttpPost("login-cookie")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken] // <--- THÊM DÒNG NÀY ĐỂ BẢO MẬT
    public async Task<IActionResult> LoginCookie([FromForm] AppLoginDTO dto, [FromQuery] string? returnUrl = "/")
    {
        try
        {
            await _auth.LoginCookie(dto);
            return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? "/dashboard" : returnUrl);
        }
        catch (Exception ex)
        {
            // form login thì redirect kèm lỗi cho dễ hiển thị
            return Redirect($"/sign-in?error=1&msg={Uri.EscapeDataString(ex.Message)}");
        }
    }

    [HttpPost("logout-cookie")]
    [Authorize(AuthenticationSchemes = "Identity.Application")]
    [ValidateAntiForgeryToken] // <--- Bật tính năng chống giả mạo request
    public async Task<IActionResult> LogoutCookie([FromQuery] string? returnUrl = "/sign-in")
    {
        await _auth.LogoutCookie();

        // chống open-redirect
        if (string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl))
            returnUrl = "/sign-in";

        return LocalRedirect(returnUrl);
    }

    // ===================== JWT =====================
    // Gọi bằng JSON từ SPA/Mobile: application/json
    // Sử dụng với mục đích API token truy cập bằng JWT ra bên ngoài
    [HttpPost("login-jwt")]
    [AllowAnonymous] // Chỉ cần cái này để ai cũng vào đăng nhập được
    [IgnoreAntiforgeryToken] // Tha cho Mobile/SPA, không bắt thẻ CSRF
    public async Task<IActionResult> LoginJwt([FromBody] AppLoginDTO dto)
    {
        try
        {
            var token = await _auth.LoginJwt(dto);

            // Trả về một object JSON đàng hoàng giúp Mobile/SPA dễ đọc dữ liệu hơn
            return Ok(new
            {
                success = true,
                accessToken = token,
                tokenType = "Bearer"
            });
        }
        catch (Exception ex)
        {
            // Trả về cấu trúc lỗi chuẩn JSON
            return Unauthorized(new
            {
                success = false,
                message = "Đăng nhập thất bại: " + ex.Message
            });
        }
    }


    // ===================== REGISTER =====================
    //Dùng register-cookie khi: đăng ký xong, server đăng nhập luôn, browser nhận cookie ngay, redirect về trang chủ
    [HttpPost("register-cookie")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterCookie([FromForm] AppRegisterDTO dto, [FromQuery] string? returnUrl = "/")
    {
        try
        {
            var rs = await _auth.RegisterAsync(dto);
            if (!rs.Succeeded)
            {
                var errors = string.Join("; ", rs.Errors.Select(x => x.Description));
                return Redirect($"/dang-ky?error=1&msg={Uri.EscapeDataString(errors)}");
            }

            await _auth.LoginCookie(new AppLoginDTO
            {
                Email = dto.Email,
                Password = dto.Password
            });

            if (string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl))
                returnUrl = "/";

            return LocalRedirect(returnUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RegisterCookie failed for email {Email}", dto.Email);
            return Redirect($"/dang-ky?error=1&msg={Uri.EscapeDataString(ex.Message)}");

        }
    }

    //dùng cho EditForm nhưng đăng ký xong luôn chuyển về trang login
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] AppRegisterDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors.Select(e => e.ErrorMessage))
                    .ToArray();

                return BadRequest(new
                {
                    success = false,
                    errors = modelErrors
                });
            }

            var rs = await _auth.RegisterAsync(dto);

            if (!rs.Succeeded)
            {
                return BadRequest(new
                {
                    success = false,
                    errors = rs.Errors.Select(x => x.Description).ToArray()
                });
            }

            return Ok(new
            {
                success = true,
                message = "Đăng ký thành công."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Register failed for email {Email}", dto.Email);

            return BadRequest(new
            {
                success = false,
                errors = new[] { ex.Message }
            });
        }
    }

}

