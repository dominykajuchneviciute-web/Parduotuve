namespace EShop.Api.Models.Items;

public class Transport : Item
{
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public int? Year { get; set; }

    public Transport()
    {
    }

    public Transport(string name, ItemCondition condition, string? manufacturer = null, string? model = null, int? year = null, string? description = null) : base(name, condition, description)
    {
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
    }
}