namespace EShop.Api.DTOs.Chat;

public class MessageDto
{
    public int Id { get; set; }
    public int ConversationId { get; set; }
    public string SenderId { get; set; } = null!;
    public string Text { get; set; } = null!;
    public DateTime SentAt { get; set; }
}