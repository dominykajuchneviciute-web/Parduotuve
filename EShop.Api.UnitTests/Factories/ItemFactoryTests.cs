using EShop.Api.DTO;
using EShop.Api.Factories;
using EShop.Api.Models;
using EShop.Api.Models.Items;

namespace EShop.Api.UnitTests.Factories;

public class ItemFactoryTests
{
    [Theory]
    [InlineData("Clothing", typeof(Clothing))]
    [InlineData("Electronics", typeof(Electronics))]
    [InlineData("Furniture", typeof(Furniture))]
    [InlineData("Transport", typeof(Transport))]
    [InlineData("Footwear", typeof(Footwear))]
    [InlineData("Instrument", typeof(Instrument))]
    public void Create_ValidCategory_ReturnsExpectedItemType(string category, Type expectedType)
    {
        var request = new CreateItemRequest
        {
            Category = category,
            Name = "Testine preke",
            Condition = ItemCondition.New
        };

        var result = ItemFactory.Create(request);

        Assert.IsType(expectedType, result);
        Assert.Equal("Testine preke", result.Name);
        Assert.Equal(ItemCondition.New, result.Condition);
    }

    [Fact]
    public void Create_ClothingWithSpecificProperties_MapsAllFieldsCorrectly()
    {
        var request = new CreateItemRequest
        {
            Category = "clothing",
            Name = "Žieminė striukė",
            Description = "Labai šilta",
            Condition = ItemCondition.Good,
            Color = "Juoda",
            Manufacturer = "Nike"
        };

        var result = Assert.IsType<Clothing>(ItemFactory.Create(request));

        Assert.Equal("Žieminė striukė", result.Name);
        Assert.Equal("Labai šilta", result.Description);
        Assert.Equal(ItemCondition.Good, result.Condition);
        Assert.Equal("Juoda", result.Color);
        Assert.Equal("Nike", result.Manufacturer);
    }

    [Fact]
    public void Create_UnknownCategory_ThrowsArgumentException()
    {
        var request = new CreateItemRequest
        {
            Category = "NezinomaKategorija",
            Name = "Testine preke"
        };

        Assert.Throws<ArgumentException>(() => ItemFactory.Create(request));
    }
}