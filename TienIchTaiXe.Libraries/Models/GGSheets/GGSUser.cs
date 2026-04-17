namespace TienIchTaiXe.Libraries.Models.GGSheets;

public class GGSUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserName { get; set; } = string.Empty;
    public string FullName   { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string HashPassword { get; set; } = string.Empty;
}


