namespace EShop.Api.Models.Chat;

public class Message
{
    public int Id { get; set; }
    public int ConversationId { get; set; }
    public string SenderId { get; set; } = null!;
    public string Text { get; set; } = null!;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}