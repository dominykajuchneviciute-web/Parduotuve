namespace EShop.Api.Models;

public class Comment
{
    public int id { get; set; }
    public string text { get; set; } = string.Empty;
    public DateTime createdAt { get; set; } = DateTime.UtcNow;

    // Ryšys su daiktu ir vartotoju
    public int itemId { get; set; }
    public Item? item { get; set; }

    public int userId { get; set; }
    public User? user { get; set; }
}