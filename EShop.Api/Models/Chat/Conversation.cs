using Microsoft.AspNetCore.Identity;

namespace EShop.Api.Models.Chat;

public record Conversation 
{
    public int Id { get; set; }
    public string User1Id { get; set; } 
    public string User2Id { get; set; }
    public DateTime? User1DeletedAt { get; set; }
    public DateTime? User2DeletedAt { get; set; }
    public int ItemId { get; set; }
    public List<Message> Messages { get; set; } = new();
}