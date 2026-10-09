
using EShop.Api.Models;

namespace EShop.Api.DTOs;

public class CreateItemRequest
{
    public string Category { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public ItemCondition Condition { get; set; }
    public string? Description { get; set; }

    public int? Size { get; set; }
    public string? Color { get; set; }
    public string? Manufacturer { get; set; }

    public string? Model { get; set; }

    public string? Material { get; set; }
    public string? Dimensions { get; set; }

    public int? Year { get; set; }

    public int? ShoeSize { get; set; }

    public string? InstrumentType { get; set; }
}