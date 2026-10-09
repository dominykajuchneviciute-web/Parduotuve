namespace EShop.Api.Models.Items;

public class Instrument : Item
{
    public string? InstrumentType { get; set; }
    public string? Manufacturer { get; set; }
    public int? Year { get; set; }

    public Instrument()
    {
    }

    public Instrument(string name, ItemCondition condition, string? instrumentType = null, string? manufacturer = null, int? year = null, string? description = null) : base(name, condition, description)
    {
        InstrumentType = instrumentType;
        Manufacturer = manufacturer;
        Year = year;
    }
}