namespace EShop.Api.Models;

public class Category
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;

    // Ryšys su prekėmis
    public List<Item> items { get; set; } = new();
}