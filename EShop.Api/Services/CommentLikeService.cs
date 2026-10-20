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
        var rawComments = await _context.ItemComments.Where(c => c.ItemId == itemId).ToListAsync();
        return rawComments.Where(c => !string.IsNullOrWhiteSpace(c.Text)).OrderBy(c => c).ToList();
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

    public async Task<bool> DeleteCommentAsync(int commentId, string userId)
    {
        var comment = await _context.ItemComments.FirstOrDefaultAsync(c => c.Id == commentId && c.UserId == userId);

        if (comment is null)
        {
            return false;
        }

        _context.ItemComments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleLikeAsync(int itemId, string userId)
    {
        var existingLike = await _context.ItemLikes.FirstOrDefaultAsync(l => l.ItemId == itemId && l.UserId == userId);

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

    public async Task<bool> ToggleSavedAsync(int itemId, string userId)
    {
        var existingSaved = await _context.ItemSaved.FirstOrDefaultAsync(s => s.ItemId == itemId && s.UserId == userId);

        if (existingSaved != null)
        {
            _context.ItemSaved.Remove(existingSaved);
            await _context.SaveChangesAsync();
            return false;
        }

        _context.ItemSaved.Add(new ItemSaved { ItemId = itemId, UserId = userId, SavedAt = DateTime.UtcNow });
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsItemSavedAsync(int itemId, string? userId)
    {
        if (string.IsNullOrEmpty(userId)) return false;
        return await _context.ItemSaved.AnyAsync(s => s.ItemId == itemId && s.UserId == userId);
    }

    public async Task<List<Item>> GetUserSavedItemsAsync(string userId)
    {
        var savedIds = await _context.ItemSaved.Where(s => s.UserId == userId).Select(s => s.ItemId).ToListAsync();
        return await _context.Items.Where(i => savedIds.Contains(i.Id)).ToListAsync();
    }
}