using System.Text;
using EShop.Api.Controllers;
using EShop.Api.DTO;
using EShop.Api.Models;
using EShop.Api.Models.Items;
using EShop.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EShop.Api.UnitTests.Controllers;

public class ItemsControllerTests
{
    [Fact]
    public async Task CreateItem_ValidRequest_ReturnsCreatedAtAction()
    {
        var mockItemService = new Mock<IItemService>();
        var mockFileLoader = new Mock<IItemFileLoader>();
        mockItemService
            .Setup(s => s.AddAsync(It.IsAny<Item>()))
            .ReturnsAsync((Item item) => { item.Id = 42; return item; });

        var controller = new ItemsController(mockItemService.Object, mockFileLoader.Object)
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
        Assert.Equal(42, item.Id);
    }

    [Fact]
    public async Task CreateItem_EmptyName_ReturnsBadRequest()
    {
        var mockItemService = new Mock<IItemService>();
        var mockFileLoader = new Mock<IItemFileLoader>();
        var controller = new ItemsController(mockItemService.Object, mockFileLoader.Object)
        {
            ControllerContext = TestHelpers.CreateControllerContext("user-1")
        };

        var result = await controller.CreateItem(new CreateItemRequest { Category = "Clothing", Name = "" });

        Assert.IsType<BadRequestObjectResult>(result.Result);
        mockItemService.Verify(s => s.AddAsync(It.IsAny<Item>()), Times.Never);
    }

    [Fact]
    public async Task GetItem_NonExistent_ReturnsNotFound()
    {
        var mockItemService = new Mock<IItemService>();
        var mockFileLoader = new Mock<IItemFileLoader>();
        mockItemService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((Item?)null);

        var controller = new ItemsController(mockItemService.Object, mockFileLoader.Object);

        var result = await controller.GetItem(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task UploadItems_ValidFile_SetsOwnerIdAndReturnsOk()
    {
        var mockItemService = new Mock<IItemService>();
        var mockFileLoader = new Mock<IItemFileLoader>();

        var loadedItems = new List<Item> { new Clothing { Name = "Ikeltas" } };
        mockFileLoader.Setup(l => l.LoadAsync(It.IsAny<Stream>())).ReturnsAsync(loadedItems);
        mockItemService.Setup(s => s.AddRangeAsync(loadedItems)).ReturnsAsync(loadedItems);

        var controller = new ItemsController(mockItemService.Object, mockFileLoader.Object)
        {
            ControllerContext = TestHelpers.CreateControllerContext("user-99")
        };

        var bytes = Encoding.UTF8.GetBytes("[]");
        var file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "file", "items.json");

        var result = await controller.UploadItems(file);

        var ok = Assert.IsType<OkObjectResult>(result);
        var items = Assert.IsType<List<Item>>(ok.Value);
        Assert.Single(items);
        Assert.Equal("user-99", items[0].OwnerId);
    }
}