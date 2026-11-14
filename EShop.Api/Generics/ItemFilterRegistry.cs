using EShop.Api.Models;

namespace EShop.Api.Generics;

public class ItemFilterRegistry<TBase>
    where TBase : Item
{
    private readonly Dictionary<
        string,
        Func<IQueryable<TBase>, ItemFilter, IQueryable<TBase>>
    > _filters = new(StringComparer.OrdinalIgnoreCase);

    public void Register<TItem>(
        string itemType,
        Func<IQueryable<TItem>, ItemFilter, IQueryable<TItem>> filter)
        where TItem : TBase
    {
        _filters[itemType] = (query, options) =>
            filter(query.OfType<TItem>(), options).Cast<TBase>();
    }

    public IQueryable<TBase> Apply(
        IQueryable<TBase> query,
        string? itemType,
        ItemFilter options)
    {
        if (string.IsNullOrWhiteSpace(itemType))
            return query;

        if (!_filters.TryGetValue(itemType, out var filter))
        {
            throw new ArgumentException(
                $"Unknown item type: {itemType}");
        }

        return filter(query, options);
    }
}