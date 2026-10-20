namespace EShop.Api.Models;

public class ItemLike
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public string UserId { get; set; } = string.Empty;
}