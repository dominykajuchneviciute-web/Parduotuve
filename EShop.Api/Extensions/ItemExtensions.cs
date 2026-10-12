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
        return true;
    }
}