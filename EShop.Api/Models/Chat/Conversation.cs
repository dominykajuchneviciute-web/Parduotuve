using Microsoft.AspNetCore.Identity;

namespace EShop.Api.Models.Chat;

public class Conversation //record geriau
{
    public int Id { get; set; }
    public string User1Id { get; set; } = null!;
    public string User2Id { get; set; } = null!; //negali likt, naudoti guid, neprasminga null reiksme
    public DateTime? User1DeletedAt { get; set; }
    public DateTime? User2DeletedAt { get; set; }
    public int ItemId { get; set; }
    public List<Message> Messages { get; set; } = new();
}