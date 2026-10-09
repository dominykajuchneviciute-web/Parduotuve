namespace EShop.Api.Models.Items;

public class Electronics : Item
{
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }

    public Electronics()
    {
    }

    public Electronics(string name, ItemCondition condition, string? manufacturer = null, string? model = null, string? color = null, string? description = null) : base(name, condition, description)
    {
        Manufacturer = manufacturer;
        Model = model;
        Color = color;
    }
}