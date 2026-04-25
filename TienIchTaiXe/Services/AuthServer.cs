using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using TienIchTaiXe.Libraries.Entities;
using TienIchTaiXe.Libraries.Models;
using TienIchTaiXe.Services.Interfaces;

namespace TienIchTaiXe.Services;

public class AuthServer : IAuthServer
{

    //User Manager
    protected readonly UserManager<AppUser> userManager;
    protected readonly SignInManager<AppUser> loginManager;
    private readonly RoleManager<IdentityRole> roleManager;
    protected readonly IConfiguration configuration;

    //Constructor
    public AuthServer(UserManager<AppUser> _userManager, SignInManager<AppUser> _loginManager, RoleManager<IdentityRole> _roleManager,
                        IConfiguration _configuration)
    {
        userManager = _userManager;
        loginManager = _loginManager;
        configuration = _configuration;
        roleManager = _roleManager;
    }

    #region Authentication với cả Cookie và JWT
    /* Đăng nhập */
    // Cookie sử dụng cho web nội bộ
    public async Task<AppUser> LoginCookie(AppLoginDTO login)
    {
        var user = await userManager.FindByEmailAsync(login.Email);
        if (user == null) throw new Exception("Wrong Email or Password");

        var result = await loginManager.PasswordSignInAsync(
            user.UserName!, login.Password,
            isPersistent: true,
            lockoutOnFailure: true); // nội bộ thì bật lockout cũng được

        if (result.IsLockedOut) throw new Exception("Tài khoản bị khóa tạm thời.");
        if (result.RequiresTwoFactor) throw new Exception("Yêu cầu xác thực 2 lớp.");
        if (!result.Succeeded) throw new Exception("Wrong Email or Password");

        return user;
    }
    // Bổ sung LogoutCookie() trong AuthServer (để controller gọi thống nhất)
    public async Task LogoutCookie()
    {
        await loginManager.SignOutAsync();
    }

    //==========================================================
    // Jwt sử dụng cho SPA hoặc Mobile
    public async Task<string> LoginJwt(AppLoginDTO login)
    {
        var user = await userManager.FindByEmailAsync(login.Email);
        if (user == null) throw new Exception("Wrong Email or Password");

        var result = await loginManager.CheckPasswordSignInAsync(user, login.Password, lockoutOnFailure: true);
        if (result.IsLockedOut) throw new Exception("Tài khoản bị khóa tạm thời.");
        if (!result.Succeeded) throw new Exception("Wrong Email or Password");

        var role = await GetRoleName(user);

        var userClaim = new InfomationUserSaveInToken
        {
            id = user.Id ?? "",
            email = user.Email ?? "",
            name = $"{user.FirstName} {user.LastName}".Trim(),
            giveName = $"{user.FirstName} {user.LastName}".Trim(),
            userName = user.UserName ?? "",
            userRole = role,
            userGuiId = Guid.NewGuid().ToString()
        };

        return await CreateToken(userClaim);
    }
    #endregion

    /* Đăng ký */
    public async Task<IdentityResult> RegisterAsync(AppRegisterDTO register, CancellationToken ct = default)
    {
        // Validate business
        if (register.Password != register.ConfirmPassword)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Mật khẩu xác nhận không khớp."
            });
        }

        var email = register.Email.Trim().ToLowerInvariant();
        var firstName = register.FirstName.Trim();
        var lastName = register.LastName.Trim();
        var gender = string.IsNullOrWhiteSpace(register.Gender) ? null : register.Gender.Trim();
        var phone = NormalizeVietnamPhone(register.PhoneNumber);

        if (string.IsNullOrWhiteSpace(phone))
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Số điện thoại không hợp lệ."
            });
        }

        // Check email exists
        var existedByEmail = await userManager.FindByEmailAsync(email);
        if (existedByEmail is not null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Email đã tồn tại."
            });
        }

        // Check phone exists
        var existedByPhone = await userManager.Users
            .AsNoTracking()
            .AnyAsync(x => x.PhoneNumber == phone, ct);

        if (existedByPhone)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Số điện thoại đã tồn tại."
            });
        }

        var newUser = new AppUser
        {
            Email = email,
            UserName = email,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phone,
            Gender = gender,
            Status = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Create user
        var createUser = await userManager.CreateAsync(newUser, register.Password);
        if (!createUser.Succeeded)
            return createUser;

        // Default role
        const string defaultRole = "Buyer";

        var roleExists = await roleManager.RoleExistsAsync(defaultRole);
        if (!roleExists)
        {
            // rollback user nếu role chưa tồn tại
            await userManager.DeleteAsync(newUser);

            return IdentityResult.Failed(new IdentityError
            {
                Description = $"Role mặc định '{defaultRole}' chưa tồn tại."
            });
        }

        var addRoleResult = await userManager.AddToRoleAsync(newUser, defaultRole);
        if (!addRoleResult.Succeeded)
        {
            // rollback user nếu add role lỗi
            await userManager.DeleteAsync(newUser);
            return addRoleResult;
        }

        return IdentityResult.Success;
    }

    private static string NormalizeVietnamPhone(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var value = input.Trim();

        // Bỏ khoảng trắng, dấu chấm, dấu gạch ngang
        value = value.Replace(" ", "")
                     .Replace(".", "")
                     .Replace("-", "");

        // +84xxxxxxxxx -> 0xxxxxxxxx
        if (value.StartsWith("+84"))
            value = "0" + value[3..];

        // Chỉ chấp nhận 4 nhà mạng: Viettel, MobiFone, VinaPhone, Vietnamobile
        var regex = new Regex(
            @"^(?:0)(?:32|33|34|35|36|37|38|39|86|96|97|98|70|76|77|78|79|89|90|93|81|82|83|84|85|88|91|94|52|56|58|92)\d{7}$",
            RegexOptions.Compiled);

        return regex.IsMatch(value) ? value : string.Empty;
    }
    //-------------------------------------

    /* Tạo token*/
    private async Task<string> CreateToken(InfomationUserSaveInToken user)
    {
        try
        {
            //Thông tin User đưa vào Token
            var listClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.id),
                            new Claim(ClaimTypes.Name, user.userName),
                            new Claim("email", user.email),
                            new Claim("name", user.name),
                            new Claim("give_name", user.giveName),
                            new Claim(ClaimTypes.Role, user.userRole),
                            new Claim(JwtRegisteredClaimNames.Jti, user.userGuiId)
                        };

            //Khóa bí mật
            var autKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]
                ?? throw new InvalidOperationException("Can't found [Secret Key] in appsettings.json !")));

            //Tạo chữ ký với khóa bí mật
            var signCredentials = new SigningCredentials(autKey, SecurityAlgorithms.HmacSha512Signature);

            var autToken = new JwtSecurityToken(
                claims: listClaims, //Thông tin User
                issuer: configuration["JWT:ValidIssuer"], //In file appsetting.json
                audience: configuration["JWT:ValidAudience"], //In file appsetting.json
                expires: DateTime.UtcNow.AddDays(30), //Thời gian tồn tại Token
                signingCredentials: signCredentials //Chữ ký
            );

            return new JwtSecurityTokenHandler().WriteToken(autToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /* Lấy thông tin quyền truy cập */
    private async Task<string> GetRoleName(AppUser user)
    {
        try
        {
            //Lấy Role của User
            var userRoles = await userManager.GetRolesAsync(user);
            var rolename = userRoles.Select(e => e).FirstOrDefault();
            return rolename == null ? string.Empty : rolename;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    // NEW FOR PROFILE
    public async Task<AccountProfileDto> GetProfileAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId)
                   ?? throw new InvalidOperationException("Không tìm thấy tài khoản.");

        return new AccountProfileDto
        {
            UserId = user.Id,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Gender = user.Gender,
            BirthDay = user.BirthDay,
            Address = user.Address,
            Status = user.Status,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    public async Task UpdateProfileAsync(string userId, AccountProfileUpdateDto dto, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId)
                   ?? throw new InvalidOperationException("Không tìm thấy tài khoản.");

        user.FirstName = dto.FirstName?.Trim() ?? "";
        user.LastName = dto.LastName?.Trim() ?? "";
        user.PhoneNumber = dto.PhoneNumber?.Trim() ?? "";
        user.Gender = string.IsNullOrWhiteSpace(dto.Gender) ? null : dto.Gender.Trim();
        user.BirthDay = dto.BirthDay;
        user.Address = string.IsNullOrWhiteSpace(dto.Address) ? null : dto.Address.Trim();
        user.UpdatedAt = DateTime.UtcNow;

        var rs = await userManager.UpdateAsync(user);
        if (!rs.Succeeded)
            throw new InvalidOperationException(string.Join("; ", rs.Errors.Select(e => e.Description)));
    }

    public async Task ChangePasswordAsync(string userId, ChangePasswordDto dto, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId)
                   ?? throw new InvalidOperationException("Không tìm thấy tài khoản.");

        var rs = await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!rs.Succeeded)
            throw new InvalidOperationException(string.Join("; ", rs.Errors.Select(e => e.Description)));
    }
}