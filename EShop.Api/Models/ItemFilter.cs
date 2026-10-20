namespace EShop.Api.Models;

public record ItemFilter(
    string? Name = null,
    ItemCondition? Condition = null,
    string? ItemType = null,
    int? Size = null,
    int? ShoeSize = null,
    int? Year = null,
    string? Manufacturer = null,
    string? Model = null,
    string? Color = null,
    string? Material = null,
    string? Dimensions = null,
    string? InstrumentType = null
);