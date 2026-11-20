using EShop.Api.Models;
using EShop.Api.Models.Items;
using EShop.Api.Services;

namespace EShop.Api.UnitTests.Services;

public class ReportServiceTests
{
    [Fact]
    public async Task ReportItemAsync_ValidAndDuplicate_WorksAndThrowsModerationException()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        context.Items.Add(new Clothing { Id = 1, Name = "Preke", OwnerId = "owner-1" });
        await context.SaveChangesAsync();

        var service = new ReportService(context);

        await service.ReportItemAsync(1, "reporter-1", "Spamas");
        var activeReports = await service.GetReportedItemsAsync();
        Assert.Single(activeReports);

        await Assert.ThrowsAsync<ModerationException>(() =>
            service.ReportItemAsync(1, "reporter-1", "Dar karta"));

        var reportId = activeReports[0].Reports[0].Id;
        Assert.True(await service.DismissAsync(reportId));
        Assert.Empty(await service.GetReportedItemsAsync());
    }

    [Fact]
    public async Task ReportCommentAsync_OwnComment_ThrowsModerationException()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        context.ItemComments.Add(new ItemComment { Id = 10, ItemId = 1, UserId = "user-1", Text = "Mano" });
        await context.SaveChangesAsync();

        var service = new ReportService(context);

        await Assert.ThrowsAsync<ModerationException>(() =>
            service.ReportCommentAsync(10, "user-1", "Priezastis"));
    }
}