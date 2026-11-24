using EShop.Api.DTOs.Chat;

namespace EShop.Api.Services.Interfaces;

public interface IChatService
{
    Task<ChatResult<int>> CreateConversationAsync(int itemId, string currentUserId);
    Task<List<ConversationDto>> GetConversationsAsync(string currentUserId);
    Task<ChatResult<List<MessageDto>>> GetMessagesAsync(int conversationId, string currentUserId);
    Task<ChatResult<MessageDto>> SendMessageAsync(int conversationId, string currentUserId, string messageText);
    Task<ChatResult<bool>> DeleteConversationAsync(int conversationId, string currentUserId);
}