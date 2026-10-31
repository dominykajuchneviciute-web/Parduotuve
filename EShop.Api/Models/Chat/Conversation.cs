using Microsoft.AspNetCore.Identity;

namespace EShop.Api.Models.Chat;

public class Conversation
{
    public int Id { get; set; }
    public string User1Id { get; set; } = null!;
    public string User2Id { get; set; } = null!;
    public int ItemId { get; set; }
    public List<Message> Messages { get; set; } = new();
}