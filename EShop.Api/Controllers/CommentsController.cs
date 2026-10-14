using System.Security.Claims;
using EShop.Api.DTO;
using EShop.Api.Models;
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

    [HttpGet("comments")]
    public async Task<ActionResult<List<ItemComment>>> GetComments(int itemId)
    {
        var comments = await _service.GetCommentsByItemAsync(itemId);
        return Ok(comments);
    }

    [Authorize]
    [HttpPost("comments")]
    public async Task<ActionResult<ItemComment>> AddComment(int itemId, [FromBody] CreateCommentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
            return BadRequest("Komentaro tekstas negali būti tuščias.");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var comment = await _service.AddCommentAsync(itemId, userId, request.Text);

        return Ok(comment);
    }

    [Authorize]
    [HttpPost("like")]
    public async Task<IActionResult> ToggleLike(int itemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var isLiked = await _service.ToggleLikeAsync(itemId, userId);
        var totalLikes = await _service.GetLikesCountAsync(itemId);

        return Ok(new { Liked = isLiked, TotalLikes = totalLikes });
    }

    [HttpGet("likes")]
    public async Task<ActionResult<int>> GetLikesCount(int itemId)
    {
        var count = await _service.GetLikesCountAsync(itemId);
        return Ok(new { ItemId = itemId, TotalLikes = count });
    }
}