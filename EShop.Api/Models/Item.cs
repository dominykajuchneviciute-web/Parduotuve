namespace EShop.Api.Models;

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