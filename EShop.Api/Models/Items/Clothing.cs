namespace EShop.Api.Models.Items;

public class Clothing : Item
{
    public int? Size { get; set; }
    public string? Color { get; set; }
    public string? Manufacturer { get; set; }

    public Clothing()
    {
    }

    public Clothing(string name, ItemCondition condition, int? size = null, string? color = null, string? manufacturer = null, string? description = null) : base(name, condition, description)
    {
        Size = size;
        Color = color;
        Manufacturer = manufacturer;
    }
}