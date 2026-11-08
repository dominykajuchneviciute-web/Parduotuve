using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using EShop.Api.Data;
using EShop.Api.Models.Chat;
using EShop.Api.DTOs.Chat;
using EShop.Api.Hubs;

namespace EShop.Api.Services;

public class ChatService
{
    private readonly AppDbContext _context;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatService(
        AppDbContext context,
        IHubContext<ChatHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }
    public async Task<ChatResult<int>> CreateConversationAsync(
        int itemId,
        string currentUserId)
    {
        var item = await _context.Items
            .FirstOrDefaultAsync(i => i.Id == itemId);

        if (item == null)
            return ChatResult<int>.NotFound("Prekė nerasta.");

        if (string.IsNullOrWhiteSpace(item.OwnerId))
            return ChatResult<int>.BadRequest(
                "Prekė neturi priskirto savininko.");

        if (item.OwnerId == currentUserId)
            return ChatResult<int>.BadRequest(
                "Negalite pradėti pokalbio su savimi.");

        var existingConversation = await _context.Conversations
            .FirstOrDefaultAsync(c =>
                c.ItemId == itemId &&
                ((c.User1Id == currentUserId &&
                  c.User2Id == item.OwnerId) ||
                 (c.User1Id == item.OwnerId &&
                  c.User2Id == currentUserId)));

        if (existingConversation != null)
        {
            existingConversation.User1DeletedAt = null;
            existingConversation.User2DeletedAt = null;

            await _context.SaveChangesAsync();

            return ChatResult<int>.Success(existingConversation.Id);
        }

        var conversation = new Conversation
        {
            User1Id = currentUserId,
            User2Id = item.OwnerId,
            ItemId = itemId
        };

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync();

        return ChatResult<int>.Success(conversation.Id);
    }

    public async Task<List<ConversationDto>> GetConversationsAsync(
        string currentUserId)
    {
        return await _context.Conversations
            .Where(c =>
                (c.User1Id == currentUserId &&
                 c.User1DeletedAt == null) ||
                (c.User2Id == currentUserId &&
                 c.User2DeletedAt == null))
            .Select(c => new ConversationDto
            {
                Id = c.Id,
                User1Id = c.User1Id,
                User2Id = c.User2Id,
                ItemId = c.ItemId,
                ItemName = _context.Items
                    .Where(i => i.Id == c.ItemId)
                    .Select(i => i.Name)
                    .FirstOrDefault()
            })
            .ToListAsync();
    }

    public async Task<ChatResult<List<MessageDto>>> GetMessagesAsync(
        int conversationId,
        string currentUserId)
    {
        var conversation = await _context.Conversations
            .FirstOrDefaultAsync(c =>
                c.Id == conversationId &&
                (c.User1Id == currentUserId ||
                 c.User2Id == currentUserId));

        if (conversation == null)
            return ChatResult<List<MessageDto>>.NotFound(
                "Pokalbis nerastas.");

        var messages = await _context.Messages
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.SentAt)
            .Select(m => new MessageDto
            {
                Id = m.Id,
                ConversationId = m.ConversationId,
                SenderId = m.SenderId,
                Text = m.Text,
                SentAt = m.SentAt,
                IsMine = m.SenderId == currentUserId
            })
            .ToListAsync();

        return ChatResult<List<MessageDto>>.Success(messages);
    }

    public async Task<ChatResult<MessageDto>> SendMessageAsync(
        int conversationId,
        string currentUserId,
        string messageText)
    {
        if (string.IsNullOrWhiteSpace(messageText))
            return ChatResult<MessageDto>.BadRequest(
                "Žinutė negali būti tuščia.");

        var conversation = await _context.Conversations
            .FirstOrDefaultAsync(c =>
                c.Id == conversationId &&
                (c.User1Id == currentUserId ||
                 c.User2Id == currentUserId));

        if (conversation == null)
            return ChatResult<MessageDto>.NotFound(
                "Pokalbis nerastas.");

        conversation.User1DeletedAt = null;
        conversation.User2DeletedAt = null;

        var message = new Message
        {
            ConversationId = conversationId,
            SenderId = currentUserId,
            Text = messageText.Trim(),
            SentAt = DateTime.UtcNow
        };

        _context.Messages.Add(message);

        await _context.SaveChangesAsync();

        var result = new MessageDto
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            SenderId = message.SenderId,
            Text = message.Text,
            SentAt = message.SentAt,
            IsMine = true
        };

        await _hubContext.Clients
            .Group($"conversation_{conversationId}")
            .SendAsync("ReceiveMessage", result);

        return ChatResult<MessageDto>.Success(result);
    }

    public async Task<ChatResult<bool>> DeleteConversationAsync(
        int conversationId,
        string currentUserId)
    {
        var conversation = await _context.Conversations
            .FirstOrDefaultAsync(c =>
                c.Id == conversationId &&
                (c.User1Id == currentUserId ||
                 c.User2Id == currentUserId));

        if (conversation == null)
            return ChatResult<bool>.NotFound(
                "Pokalbis nerastas.");

        if (conversation.User1Id == currentUserId)
            conversation.User1DeletedAt = DateTime.UtcNow;
        else
            conversation.User2DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ChatResult<bool>.Success(true);
    }
}

public record ChatResult<T>(
    T? Value,
    string? Error,
    bool IsNotFound,
    bool IsBadRequest)
{
    public static ChatResult<T> Success(T value) =>
        new(value, null, false, false);

    public static ChatResult<T> NotFound(string error) =>
        new(default, error, true, false);

    public static ChatResult<T> BadRequest(string error) =>
        new(default, error, false, true);
}
