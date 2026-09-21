using EShop.Api.Enums;

namespace EShop.Api.Models;

public class Item
{
    public int id { get; set; }
    public string title { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public decimal price { get; set; }
    public ItemCondition condition { get; set; }
    public DateTime createdAt { get; set; } = DateTime.UtcNow;

    // Svetimi raktai (Foreign Keys) ir ryšiai
    public int userId { get; set; }
    public User? user { get; set; }

    public int categoryId { get; set; }
    public Category? category { get; set; }

    public List<Comment> comments { get; set; } = new();
    public List<Like> likes { get; set; } = new();
}