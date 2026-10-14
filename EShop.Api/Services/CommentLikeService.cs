using EShop.Api.Data;
using EShop.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Api.Services;

public class CommentLikeService
{
    private readonly AppDbContext _context;

    public CommentLikeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ItemComment>> GetCommentsByItemAsync(int itemId)
    {
        var rawComments = await _context.ItemComments
            .Where(c => c.ItemId == itemId)
            .ToListAsync(); 

        return rawComments
            .Where(c => !string.IsNullOrWhiteSpace(c.Text))
            .OrderBy(c => c)
            .ToList();
    }

    public async Task<ItemComment> AddCommentAsync(int itemId, string userId, string text)
    {
        var comment = new ItemComment
        {
            ItemId = itemId,
            UserId = userId,
            Text = text,
            CreatedAt = DateTime.UtcNow
        };

        _context.ItemComments.Add(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<bool> ToggleLikeAsync(int itemId, string userId)
    {
        var existingLike = await _context.ItemLikes
            .FirstOrDefaultAsync(l => l.ItemId == itemId && l.UserId == userId);

        if (existingLike != null)
        {
            _context.ItemLikes.Remove(existingLike);
            await _context.SaveChangesAsync();
            return false;
        }

        _context.ItemLikes.Add(new ItemLike { ItemId = itemId, UserId = userId });
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetLikesCountAsync(int itemId)
    {
        return await _context.ItemLikes.CountAsync(l => l.ItemId == itemId);
    }
}