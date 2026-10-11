using System.Text.Json.Serialization;
using EShop.Api.Models.Items;


namespace EShop.Api.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "itemType")]
[JsonDerivedType(typeof(Clothing), "Clothing")]
[JsonDerivedType(typeof(Electronics), "Electronics")]
[JsonDerivedType(typeof(Furniture), "Furniture")]
[JsonDerivedType(typeof(Transport), "Transport")]
[JsonDerivedType(typeof(Footwear), "Footwear")]
[JsonDerivedType(typeof(Instrument), "Instrument")]

public abstract class Item
{
    public string Name {get; set;} = string.Empty;
    public ItemCondition Condition {get; set;}
    public int Id { get; set; }
    public string? Description {get; set;}
    
    public Item()
    {
    }

    public Item (string name, ItemCondition condition, string? description = null)
    {
        Name = name;
        Condition = condition;
        Description = description;
    }

}