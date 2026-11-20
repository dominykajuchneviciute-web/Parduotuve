using System.Text;
using EShop.Api.Controllers;
using EShop.Api.DTO;
using EShop.Api.Models;
using EShop.Api.Models.Items;
using EShop.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.UnitTests.Controllers;

public class ItemsControllerTests
{
    [Fact]
    public async Task CreateItem_ValidRequest_ReturnsCreatedAtAction()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var controller = new ItemsController(new ItemService(context), new ItemFileLoader())
        {
            ControllerContext = TestHelpers.CreateControllerContext("user-1")
        };

        var result = await controller.CreateItem(new CreateItemRequest
        {
            Category = "Clothing",
            Name = "Nauja striuke",
            Condition = ItemCondition.New
        });

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var item = Assert.IsType<Clothing>(created.Value);
        Assert.Equal("user-1", item.OwnerId);
    }

    [Fact]
    public async Task CreateItem_EmptyName_ReturnsBadRequest()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var controller = new ItemsController(new ItemService(context), new ItemFileLoader())
        {
            ControllerContext = TestHelpers.CreateControllerContext("user-1")
        };

        var result = await controller.CreateItem(new CreateItemRequest { Category = "Clothing", Name = "" });

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetItem_NonExistent_ReturnsNotFound()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var controller = new ItemsController(new ItemService(context), new ItemFileLoader());

        var result = await controller.GetItem(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task UploadItems_ValidFile_SetsOwnerIdAndReturnsOk()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        var controller = new ItemsController(new ItemService(context), new ItemFileLoader())
        {
            ControllerContext = TestHelpers.CreateControllerContext("user-99")
        };

        var json = """[{"itemType":"Clothing","name":"Ikeltas","condition":0}]""";
        var bytes = Encoding.UTF8.GetBytes(json);
        var file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "file", "items.json");

        var result = await controller.UploadItems(file);

        var ok = Assert.IsType<OkObjectResult>(result);
        var items = Assert.IsType<List<Item>>(ok.Value);
        Assert.Single(items);
        Assert.Equal("user-99", items[0].OwnerId);
    }
}