namespace EShop.Api.Models;

public class Like
{
    public int id { get; set; }

    // Ryšys su daiktu ir vartotoju
    public int itemId { get; set; }
    public Item? item { get; set; }

    public int userId { get; set; }
    public User? user { get; set; }
}