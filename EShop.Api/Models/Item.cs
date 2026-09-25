namespace EShop.Api.Models;

public class Item
{
    public string Name {get; set;}
    public ItemCondition Condition {get; set;}
    public int Id { get; set; }
    public string? Description {get; set;}
    public int? Size {get; set;}
    public string? Manufacturer {get; set;}
    public string? Color {get; set;}
    

    public Item (string name, ItemCondition condition,int id, string? description = null, int? size = null, string? manufacturer = null, string? color = null)
    {
        Name = name;
        Condition = condition;
        Id= id;
        Description = description;
        Size = size;
        Manufacturer = manufacturer;
        Color = color;
    }

}