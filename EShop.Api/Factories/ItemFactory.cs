using EShop.Api.Models;
using EShop.Api.Models.Items;
using EShop.Api.DTOs;

namespace EShop.Api.Factories;

public static class ItemFactory
{
    public static Item Create(CreateItemRequest request)
    {
        return request.Category.Trim().ToLowerInvariant() switch
        {
            "clothing" => new Clothing
            {
                Name = request.Name,
                Condition = request.Condition,
                Description = request.Description,
                Size = request.Size,
                Color = request.Color,
                Manufacturer = request.Manufacturer
            },

            "electronics" => new Electronics
            {
                Name = request.Name,
                Condition = request.Condition,
                Description = request.Description,
                Model = request.Model,
                Color = request.Color,
                Manufacturer = request.Manufacturer
            },

            "furniture" => new Furniture
            {
                Name = request.Name,
                Condition = request.Condition,
                Description = request.Description,
                Material = request.Material,
                Color = request.Color,
                Dimensions = request.Dimensions
            },

            "transport" => new Transport
            {
                Name = request.Name,
                Condition = request.Condition,
                Description = request.Description,
                Manufacturer = request.Manufacturer,
                Model = request.Model,
                Year = request.Year
            },

            "footwear" => new Footwear
            {
                Name = request.Name,
                Condition = request.Condition,
                Description = request.Description,
                ShoeSize = request.ShoeSize,
                Color = request.Color,
                Manufacturer = request.Manufacturer
            },

            "instrument" => new Instrument
            {
                Name = request.Name,
                Condition = request.Condition,
                Description = request.Description,
                InstrumentType = request.InstrumentType,
                Manufacturer = request.Manufacturer,
                Year = request.Year
            },

            _ => throw new ArgumentException(
                "Nežinoma prekės kategorija. Galimos kategorijos: " +
                "Clothing, Electronics, Furniture, Transport, Footwear, Instrument.")
        };
    }
}