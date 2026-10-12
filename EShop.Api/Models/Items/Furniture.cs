namespace EShop.Api.Models.Items;

public class Furniture : Item
{
    public string? Material { get; set; }
    public string? Color { get; set; }
    public string? Dimensions { get; set; }

    public Furniture()
    {
    }

    public Furniture(string name, ItemCondition condition, string? material = null, string? color = null, string? dimensions = null, string? description = null) : base(name, condition, description)
    {
        Material = material;
        Color = color;
        Dimensions = dimensions;
    }
}