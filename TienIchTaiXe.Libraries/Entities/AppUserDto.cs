
namespace TienIchTaiXe.Libraries.Entities;

using System.ComponentModel.DataAnnotations;
using TienIchTaiXe.Libraries.Models;

//Login: Using Regular Expression
public class AppLoginDTO
{
    [Required(ErrorMessage = "Email không được bỏ trống.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Mật khẩu không được bỏ trống.")]
    public string Password { get; set; } = string.Empty;
}

//Register
public sealed class AppRegisterDTO
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

//User profile
public sealed class AccountProfileDto
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }

    public string? Gender { get; set; }
    public DateTime? BirthDay { get; set; }
    public string? Address { get; set; }

    public short Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public sealed class AccountProfileUpdateDto
{
    [Required(ErrorMessage = "Họ không được bỏ trống.")]
    [MaxLength(50, ErrorMessage = "Họ tối đa 50 ký tự.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tên không được bỏ trống.")]
    [MaxLength(50, ErrorMessage = "Tên tối đa 50 ký tự.")]
    public string LastName { get; set; } = string.Empty;

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

public sealed class ChangePasswordDto
{
    [Required(ErrorMessage = "Mật khẩu hiện tại không được bỏ trống.")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mật khẩu mới không được bỏ trống.")]
    [MinLength(6, ErrorMessage = "Mật khẩu mới phải có ít nhất 6 ký tự.")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Xác nhận mật khẩu mới không được bỏ trống.")]
    [Compare(nameof(NewPassword), ErrorMessage = "Xác nhận mật khẩu mới không khớp.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}