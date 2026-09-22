namespace EShop.Api.Models;

public class User
{
    public int id { get; set; }
    public string username { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;

    // Ryšiai (Navigation properties)
    public List<Item> items { get; set; } = new();
    public List<Comment> comments { get; set; } = new();
    public List<Like> likes { get; set; } = new();
}