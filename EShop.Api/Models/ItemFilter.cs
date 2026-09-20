namespace EShop.Api.Models;

public record ItemFilter(
    string? Name = null,
    ItemCondition? Condition = null,
    int? Size = null,
    string? Manufacturer = null,
    string? Color = null
);