using EShop.Api.Models;

namespace EShop.Api.Services.Interfaces;

public interface ICommentLikeService
{
    Task<List<ItemComment>> GetCommentsByItemAsync(int itemId);
    Task<ItemComment> AddCommentAsync(int itemId, string userId, string text);
    Task<bool> DeleteCommentAsync(int commentId, string userId);
    Task<bool> DeleteCommentAsModeratorAsync(int commentId);
    Task<bool> ToggleLikeAsync(int itemId, string userId);
    Task<int> GetLikesCountAsync(int itemId);
    Task<bool> ToggleSavedAsync(int itemId, string userId);
    Task<bool> IsItemSavedAsync(int itemId, string? userId);
    Task<List<Item>> GetUserSavedItemsAsync(string userId);
}