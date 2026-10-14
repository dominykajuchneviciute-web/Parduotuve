using System.Security.Claims;
using EShop.Api.DTO;
using EShop.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers;

[Route("api/items/{itemId:int}")]
[ApiController]
public class ItemInteractionsController : ControllerBase
{
    private readonly CommentLikeService _service;

    public ItemInteractionsController(CommentLikeService service)
    {
        _service = service;
    }

    private string? GetCurrentUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")?.Value;

    [HttpGet("comments")]
    public async Task<ActionResult<List<CommentResponse>>> GetComments(int itemId)
    {
        var currentUserId = GetCurrentUserId();
        var comments = await _service.GetCommentsByItemAsync(itemId);

        var result = comments.Select(c => new CommentResponse(
            c.Id,
            c.ItemId,
            c.Text,
            c.CreatedAt,
            IsOwner: !string.IsNullOrEmpty(currentUserId) && c.UserId == currentUserId
        )).ToList();

        return Ok(result);
    }

    [Authorize]
    [HttpPost("comments")]
    public async Task<IActionResult> AddComment(int itemId, [FromBody] CreateCommentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
            return BadRequest("Komentaro tekstas negali būti tuščias.");

        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var comment = await _service.AddCommentAsync(itemId, userId, request.Text);

        return Ok(new CommentResponse(
            comment.Id,
            comment.ItemId,
            comment.Text,
            comment.CreatedAt,
            IsOwner: true
        ));
    }

    [Authorize]
    [HttpDelete("comments/{commentId:int}")]
    public async Task<IActionResult> DeleteComment(int itemId, int commentId)
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var deleted = await _service.DeleteCommentAsync(commentId, userId);

        if (!deleted)
        {
            return NotFound("Komentaras nerastas arba neturite teisių jo trinti.");
        }

        return NoContent();
    }

    [Authorize]
    [HttpPost("like")]
    public async Task<IActionResult> ToggleLike(int itemId)
    {
        var userId = GetCurrentUserId();
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var isLiked = await _service.ToggleLikeAsync(itemId, userId);
        var totalLikes = await _service.GetLikesCountAsync(itemId);

        return Ok(new { Liked = isLiked, TotalLikes = totalLikes });
    }

    [HttpGet("likes")]
    public async Task<IActionResult> GetLikesCount(int itemId)
    {
        var count = await _service.GetLikesCountAsync(itemId);
        return Ok(new { ItemId = itemId, TotalLikes = count });
    }
}