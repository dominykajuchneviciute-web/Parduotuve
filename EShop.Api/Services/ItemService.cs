using EShop.Api.Data;
using EShop.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Api.Services;

public class ItemService
{
    private readonly AppDbContext _context;

    public ItemService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Item>> GetAllAsync()
    {
        return await _context.Items.ToListAsync();
    }

    public async Task<Item?> GetByIdAsync(int id)
    {
        return await _context.Items
            .FirstOrDefaultAsync(item => item.Id == id);
    }

    public async Task<Item> AddAsync(Item item)
    {
        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        return item;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _context.Items
            .FirstOrDefaultAsync(item => item.Id == id);

        if (item == null)
        {
            return false;
        }

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();

        return true;
    }
}