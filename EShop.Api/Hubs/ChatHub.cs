
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EShop.Api.Data;

namespace EShop.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly AppDbContext _context;

    public ChatHub(AppDbContext context)
    {
        _context = context;
    }

    public async Task JoinConversation(int conversationId)
    {
        var currentUserId = Context.User?
            .FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserId))
            throw new HubException("Vartotojas nerastas.");

        var isParticipant = await _context.Conversations
            .AnyAsync(c =>
                c.Id == conversationId &&
                (c.User1Id == currentUserId ||
                 c.User2Id == currentUserId));

        if (!isParticipant)
            throw new HubException("Neturite prieigos prie šio pokalbio.");

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            $"conversation_{conversationId}");
    }

    public async Task LeaveConversation(int conversationId)
    {
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            $"conversation_{conversationId}");
    }
}
