using EShop.Api.Models;

namespace EShop.Api.Extensions;

public static class ItemExtensions
{
    public static bool Matches(this Item item, ItemFilter filter)
    {
        if (filter.Name != null && !item.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (filter.Condition != null && item.Condition != filter.Condition)
        {
            return false;
        }

        if (filter.Size != null && item.Size != filter.Size)
        {
            return false;
        }

        if (filter.Manufacturer != null && !string.Equals(item.Manufacturer, filter.Manufacturer, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (filter.Color != null && !string.Equals(item.Color, filter.Color, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        return true;
    }
}