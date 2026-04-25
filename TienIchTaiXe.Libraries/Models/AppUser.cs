using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TienIchTaiXe.Libraries.Models;

public class AppUser : IdentityUser
{
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    [MaxLength(20)]
    public string? Gender { get; set; } = string.Empty;
    public DateTime? BirthDay { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    public short Status { get; set; } = 1; // 1 Active, 2 Locked, 3 Deleted

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

}

//Register: set default role for new User
public partial class UserRoles
{
    public string RoleId { get; set; } = "4";
    public string RoleName { get; set; } = "User";
    public bool IsSelected { get; set; } = true;
}


//Login: create token 
public partial class InfomationUserSaveInToken
{
    public string id { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string userName { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public string giveName { get; set; } = string.Empty;
    public string userRole { get; set; } = string.Empty;
    public string userGuiId { get; set; } = string.Empty;
}

public sealed class ProfileVm
{
    [Required(ErrorMessage = "Họ không được bỏ trống.")]
    [MaxLength(50, ErrorMessage = "Họ tối đa 50 ký tự.")]
    public string FirstName { get; set; } = "";

    [Required(ErrorMessage = "Tên không được bỏ trống.")]
    [MaxLength(50, ErrorMessage = "Tên tối đa 50 ký tự.")]
    public string LastName { get; set; } = "";

    [Required(ErrorMessage = "Số điện thoại không được bỏ trống.")]
    [StringLength(12, MinimumLength = 10, ErrorMessage = "Số điện thoại phải từ 10 đến 12 ký tự.")]
    [RegularExpression(
        @"^(?:(?:\+84|0)(?:32|33|34|35|36|37|38|39|86|96|97|98|70|76|77|78|79|89|90|93|81|82|83|84|85|88|91|94|52|56|58|92)\d{7})$",
        ErrorMessage = "Số điện thoại Việt Nam không hợp lệ."
    )]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(20, ErrorMessage = "Giới tính tối đa 20 ký tự.")]
    public string? Gender { get; set; }

    public DateTime? BirthDay { get; set; }

    [MaxLength(500, ErrorMessage = "Địa chỉ tối đa 500 ký tự.")]
    public string? Address { get; set; }
}

public sealed class RegisterVm
{
    [Required(ErrorMessage = "Họ không được bỏ trống.")]
    [MaxLength(50, ErrorMessage = "Họ tối đa 50 ký tự.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tên không được bỏ trống.")]
    [MaxLength(50, ErrorMessage = "Tên tối đa 50 ký tự.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email không được bỏ trống.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Số điện thoại không được bỏ trống.")]
    [StringLength(12, MinimumLength = 10, ErrorMessage = "Số điện thoại phải từ 10 đến 12 ký tự.")]
    [RegularExpression(
        @"^(?:(?:\+84|0)(?:32|33|34|35|36|37|38|39|86|96|97|98|70|76|77|78|79|89|90|93|81|82|83|84|85|88|91|94|52|56|58|92)\d{7})$",
        ErrorMessage = "Số điện thoại Việt Nam không hợp lệ."
    )]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(20, ErrorMessage = "Giới tính tối đa 20 ký tự.")]
    public string? Gender { get; set; }

    [Required(ErrorMessage = "Mật khẩu không được bỏ trống.")]
    [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Xác nhận mật khẩu không được bỏ trống.")]
    [Compare(nameof(Password), ErrorMessage = "Mật khẩu xác nhận không khớp.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
