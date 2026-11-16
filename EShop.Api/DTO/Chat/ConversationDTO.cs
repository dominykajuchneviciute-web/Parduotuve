namespace EShop.Api.DTOs.Chat;

public record ConversationDto
{
    public int Id { get; set; }
    public string User1Id { get; set; } = string.Empty;
    public string User2Id { get; set; } = string.Empty;
    public int ItemId { get; set; }
    public string? ItemName { get; set; }
}