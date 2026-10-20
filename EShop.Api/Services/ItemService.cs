using EShop.Api.Data;
using EShop.Api.Models;
using EShop.Api.Models.Items;
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

    public async Task<List<Item>> GetFilteredAsync(ItemFilter filter)
    {
        var query = _context.Items.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var name = filter.Name.Trim();
            query = query.Where(item => EF.Functions.ILike(item.Name, $"%{name}%"));
        }

        if (filter.Condition.HasValue)
        {
            query = query.Where(item => item.Condition == filter.Condition.Value);
        }

        var itemType = filter.ItemType?.Trim().ToLowerInvariant();

        switch (itemType)
        {
            case null:
            case "":
                break;
            case "clothing":
                query = ApplyClothingFilters(query.OfType<Clothing>(), filter)
                    .Cast<Item>();
                break;
            case "electronics":
                query = ApplyElectronicsFilters(query.OfType<Electronics>(), filter)
                    .Cast<Item>();
                break;
            case "furniture":
                query = ApplyFurnitureFilters(query.OfType<Furniture>(), filter)
                    .Cast<Item>();
                break;
            case "transport":
                query = ApplyTransportFilters(query.OfType<Transport>(), filter)
                    .Cast<Item>();
                break;
            case "footwear":
                query = ApplyFootwearFilters(query.OfType<Footwear>(), filter)
                    .Cast<Item>();
                break;
            case "instrument":
                query = ApplyInstrumentFilters(query.OfType<Instrument>(), filter)
                    .Cast<Item>();
                break;
            default:
                throw new ArgumentException(
                    "Unknown item type. Allowed values: Clothing, Electronics, Furniture, Transport, Footwear, Instrument.");
        }

        return await query.ToListAsync();
    }

    public async Task<Item?> GetByIdAsync(int id)
    {
        return await _context.Items
            .FirstOrDefaultAsync(item => item.Id == id);
    }

    public async Task<List<Item>> GetByOwnerAsync(string ownerId)
    {
        return await _context.Items
            .Where(item => item.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<Item> AddAsync(Item item)
    {
        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        return item;
    }
    public async Task<List<Item>> AddRangeAsync(List<Item> items)
    {
        _context.Items.AddRange(items);
        await _context.SaveChangesAsync();

        return items;
    }

    public async Task<bool> DeleteAsync(int id, string ownerId)
    {
        var item = await _context.Items
            .FirstOrDefaultAsync(item =>
                item.Id == id && item.OwnerId == ownerId);

        if (item == null)
        {
            return false;
        }

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();

        return true;
    }

    private static IQueryable<Clothing> ApplyClothingFilters(
        IQueryable<Clothing> query,
        ItemFilter filter)
    {
        if (filter.Size.HasValue)
        {
            query = query.Where(item => item.Size == filter.Size.Value);
        }

        query = WhereContains(query, filter.Color, nameof(Clothing.Color));
        return WhereContains(query, filter.Manufacturer, nameof(Clothing.Manufacturer));
    }

    private static IQueryable<Electronics> ApplyElectronicsFilters(
        IQueryable<Electronics> query,
        ItemFilter filter)
    {
        query = WhereContains(query, filter.Manufacturer, nameof(Electronics.Manufacturer));
        query = WhereContains(query, filter.Model, nameof(Electronics.Model));
        return WhereContains(query, filter.Color, nameof(Electronics.Color));
    }

    private static IQueryable<Furniture> ApplyFurnitureFilters(
        IQueryable<Furniture> query,
        ItemFilter filter)
    {
        query = WhereContains(query, filter.Material, nameof(Furniture.Material));
        query = WhereContains(query, filter.Color, nameof(Furniture.Color));
        return WhereContains(query, filter.Dimensions, nameof(Furniture.Dimensions));
    }

    private static IQueryable<Transport> ApplyTransportFilters(
        IQueryable<Transport> query,
        ItemFilter filter)
    {
        query = WhereContains(query, filter.Manufacturer, nameof(Transport.Manufacturer));
        query = WhereContains(query, filter.Model, nameof(Transport.Model));

        if (filter.Year.HasValue)
        {
            query = query.Where(item => item.Year == filter.Year.Value);
        }

        return query;
    }

    private static IQueryable<Footwear> ApplyFootwearFilters(
        IQueryable<Footwear> query,
        ItemFilter filter)
    {
        if (filter.ShoeSize.HasValue)
        {
            query = query.Where(item => item.ShoeSize == filter.ShoeSize.Value);
        }

        query = WhereContains(query, filter.Color, nameof(Footwear.Color));
        return WhereContains(query, filter.Manufacturer, nameof(Footwear.Manufacturer));
    }

    private static IQueryable<Instrument> ApplyInstrumentFilters(
        IQueryable<Instrument> query,
        ItemFilter filter)
    {
        query = WhereContains(query, filter.InstrumentType, nameof(Instrument.InstrumentType));
        query = WhereContains(query, filter.Manufacturer, nameof(Instrument.Manufacturer));

        if (filter.Year.HasValue)
        {
            query = query.Where(item => item.Year == filter.Year.Value);
        }

        return query;
    }

    private static IQueryable<TItem> WhereContains<TItem>(
        IQueryable<TItem> query,
        string? value,
        string propertyName)
        where TItem : Item
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return query;
        }

        var searchValue = value.Trim();
        return query.Where(item =>
            EF.Functions.ILike(
                EF.Property<string>(item, propertyName),
                $"%{searchValue}%"));
    }
}