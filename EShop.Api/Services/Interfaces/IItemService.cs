using EShop.Api.Models;

namespace EShop.Api.Services.Interfaces;

public interface IItemService
{
    Task<List<Item>> GetAllAsync();
    Task<List<Item>> GetFilteredAsync(ItemFilter filter);
    Task<Item?> GetByIdAsync(int id);
    Task<List<Item>> GetByOwnerAsync(string ownerId);
    Task<Item> AddAsync(Item item);
    Task<List<Item>> AddRangeAsync(List<Item> items);
    Task<bool> DeleteAsync(int id, string ownerId);
    Task<bool> DeleteAsModeratorAsync(int id);
}