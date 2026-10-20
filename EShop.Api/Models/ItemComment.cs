namespace EShop.Api.Models;

public class ItemComment : IComparable<ItemComment>
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int CompareTo(ItemComment? other)
    {
        if (other is null) return 1;
        return CreatedAt.CompareTo(other.CreatedAt);
    }
}