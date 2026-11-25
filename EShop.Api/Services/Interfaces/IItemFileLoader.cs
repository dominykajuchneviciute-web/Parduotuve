using EShop.Api.Models;

namespace EShop.Api.Services.Interfaces;

public interface IItemFileLoader
{
    Task<List<Item>> LoadAsync(Stream stream);
}