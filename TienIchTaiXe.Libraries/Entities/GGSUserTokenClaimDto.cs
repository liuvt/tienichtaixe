using System.ComponentModel.DataAnnotations;

namespace TienIchTaiXe.Libraries.Entities;

public class GGSUserTokenClaimDto
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty; //TienIchTaiXe2025@gmail.com
    public string JwtRegisteredClaimNames { get; set; } = string.Empty;
}