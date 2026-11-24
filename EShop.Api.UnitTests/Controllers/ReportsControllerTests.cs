using EShop.Api.Controllers;
using EShop.Api.DTO;
using EShop.Api.Services;
using EShop.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EShop.Api.UnitTests.Controllers;

public class ReportsControllerTests
{
    [Fact]
    public async Task ReportItem_EmptyReason_ReturnsBadRequest()
    {
        var mockService = new Mock<IReportService>();
        var controller = new ReportsController(mockService.Object)
        {
            ControllerContext = TestHelpers.CreateControllerContext("user-1")
        };

        var result = await controller.ReportItem(1, new CreateReportRequest("  "));

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task ReportItem_DuplicateReport_ReturnsConflict()
    {
        var mockService = new Mock<IReportService>();
        mockService.Setup(s => s.ReportItemAsync(1, "user-1", "Spamas"))
            .ThrowsAsync(new ModerationException("Jau pranesta"));

        var controller = new ReportsController(mockService.Object)
        {
            ControllerContext = TestHelpers.CreateControllerContext("user-1")
        };

        var result = await controller.ReportItem(1, new CreateReportRequest("Spamas"));

        Assert.IsType<ConflictObjectResult>(result);
    }

    [Fact]
    public async Task DismissReport_ExistingReport_ReturnsNoContent()
    {
        var mockService = new Mock<IReportService>();
        mockService.Setup(s => s.DismissAsync(5)).ReturnsAsync(true);

        var controller = new ReportsController(mockService.Object);

        var result = await controller.DismissReport(5);

        Assert.IsType<NoContentResult>(result);
    }
}