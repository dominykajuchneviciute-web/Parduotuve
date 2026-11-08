using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EShop.Api.DTOs.Chat;
using EShop.Api.Services;

namespace EShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    private string GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }

    [HttpPost("conversations")]
    public async Task<IActionResult> CreateConversation(int itemId)
    {
        var result = await _chatService.CreateConversationAsync(
            itemId,
            GetCurrentUserId());

        if (result.IsNotFound)
            return NotFound(result.Error);

        if (result.IsBadRequest)
            return BadRequest(result.Error);

        return Ok(new { id = result.Value });
    }

    [HttpGet("conversations")]
    public async Task<IActionResult> GetConversations()
    {
        var conversations = await _chatService.GetConversationsAsync(
            GetCurrentUserId());

        return Ok(conversations);
    }

    [HttpGet("conversations/{conversationId}/messages")]
    public async Task<IActionResult> GetMessages(int conversationId)
    {
        var result = await _chatService.GetMessagesAsync(
            conversationId,
            GetCurrentUserId());

        if (result.IsNotFound)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpPost("conversations/{conversationId}/messages")]
    public async Task<IActionResult> SendMessage(
        int conversationId,
        [FromBody] SendMessageRequest request)
    {
        var result = await _chatService.SendMessageAsync(
            conversationId,
            GetCurrentUserId(),
            request.Text);

        if (result.IsNotFound)
            return NotFound(result.Error);

        if (result.IsBadRequest)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpDelete("conversations/{conversationId}")]
    public async Task<IActionResult> DeleteConversation(
        int conversationId)
    {
        var result = await _chatService.DeleteConversationAsync(
            conversationId,
            GetCurrentUserId());

        if (result.IsNotFound)
            return NotFound(result.Error);

        return Ok(new { message = "Pokalbis ištrintas." });
    }
}
