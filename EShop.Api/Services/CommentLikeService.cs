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