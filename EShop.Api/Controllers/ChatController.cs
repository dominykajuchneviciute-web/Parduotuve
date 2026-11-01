using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EShop.Api.Data;
using EShop.Api.Models.Chat;
using EShop.Api.DTOs.Chat;

namespace EShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly AppDbContext _context;

    public ChatController(AppDbContext context)
    {
        _context = context;
    }

    private string GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }

    [HttpPost("conversations")]
    public async Task<IActionResult> CreateConversation(
        int itemId)
    {
        var currentUserId = GetCurrentUserId();

        var item = await _context.Items
            .FirstOrDefaultAsync(i => i.Id == itemId);

        if (item == null)
            return NotFound("Prekė nerasta.");

        if (string.IsNullOrWhiteSpace(item.OwnerId))
            return BadRequest("Prekė neturi priskirto savininko.");

        if (item.OwnerId == currentUserId)
            return BadRequest("Negalite pradėti pokalbio su savimi.");

        var existingConversation = await _context.Conversations
            .FirstOrDefaultAsync(c =>
                c.ItemId == itemId &&
                ((c.User1Id == currentUserId &&
                  c.User2Id == item.OwnerId) ||
                 (c.User1Id == item.OwnerId &&
                  c.User2Id == currentUserId)));

        if (existingConversation != null)
            return Ok(new { id = existingConversation.Id });

        var conversation = new Conversation
        {
            User1Id = currentUserId,
            User2Id = item.OwnerId,
            ItemId = itemId
        };

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync();

        return Ok(new { id = conversation.Id });
    }

    [HttpGet("conversations")]
    public async Task<IActionResult> GetConversations()
    {
        var currentUserId = GetCurrentUserId();

        var conversations = await _context.Conversations
            .Where(c =>
                c.User1Id == currentUserId ||
                c.User2Id == currentUserId)
            .ToListAsync();

        return Ok(conversations);
    }

    [HttpGet("conversations/{conversationId}/messages")]
    public async Task<IActionResult> GetMessages(int conversationId)
    {
        var currentUserId = GetCurrentUserId();

        var conversation = await _context.Conversations
            .FirstOrDefaultAsync(c =>
                c.Id == conversationId &&
                (c.User1Id == currentUserId ||
                 c.User2Id == currentUserId));

        if (conversation == null)
            return NotFound("Pokalbis nerastas.");

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

        return Ok(messages);
    }

    [HttpPost("conversations/{conversationId}/messages")]
    public async Task<IActionResult> SendMessage(
        int conversationId,
        [FromBody] SendMessageRequest request)
    {
        var currentUserId = GetCurrentUserId();

        if (string.IsNullOrWhiteSpace(request.Text))
            return BadRequest("Žinutė negali būti tuščia.");

        var conversation = await _context.Conversations
            .FirstOrDefaultAsync(c =>
                c.Id == conversationId &&
                (c.User1Id == currentUserId ||
                 c.User2Id == currentUserId));

        if (conversation == null)
            return NotFound("Pokalbis nerastas.");

        var message = new Message
        {
            ConversationId = conversationId,
            SenderId = currentUserId,
            Text = request.Text.Trim(),
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

        return Ok(result);
    }
}