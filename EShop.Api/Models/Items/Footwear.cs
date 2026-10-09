namespace EShop.Api.Models.Items;

public class Footwear : Item
{
    public int? ShoeSize { get; set; }
    public string? Color { get; set; }
    public string? Manufacturer { get; set; }

    public Footwear()
    {
    }

    public Footwear(string name, ItemCondition condition, int? shoeSize = null, string? color = null, string? manufacturer = null, string? description = null) : base(name, condition, description)
    {
        ShoeSize = shoeSize;
        Color = color;
        Manufacturer = manufacturer;
    }
}