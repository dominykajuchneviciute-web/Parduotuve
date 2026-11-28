namespace EShop.Api.DTO;

public class AdminUserResponse
{
    public string Id { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public List<string> Roles { get; set; } = [];
    public bool IsBlocked { get; set; }
}