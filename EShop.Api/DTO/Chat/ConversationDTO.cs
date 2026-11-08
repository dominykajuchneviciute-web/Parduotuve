namespace EShop.Api.DTOs.Chat;

public record ConversationDto
{
    public int Id { get; set; }
    public string User1Id { get; set; } 
    public string User2Id { get; set; } 
    public int ItemId { get; set; }
    public string? ItemName { get; set; }
}