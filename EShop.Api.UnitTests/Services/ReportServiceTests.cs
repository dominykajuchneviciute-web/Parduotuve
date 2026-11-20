using EShop.Api.Models;
using EShop.Api.Models.Items;
using EShop.Api.Services;

namespace EShop.Api.UnitTests.Services;

public class ReportServiceTests
{
    [Fact]
    public async Task ReportItemAsync_ValidReport_CreatesActiveReport()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        context.Items.Add(new Clothing { Id = 1, Name = "Preke", OwnerId = "owner-1" });
        await context.SaveChangesAsync();
        var service = new ReportService(context);

        await service.ReportItemAsync(1, "reporter-1", "Spamas");
        var activeReports = await service.GetReportedItemsAsync();
        Assert.Single(activeReports);
        Assert.Equal(1, activeReports[0].ReportCount);
    }

    [Fact]
    public async Task ReportItemAsync_DuplicateReport_ThrowsModerationException()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        context.Items.Add(new Clothing { Id = 1, Name = "Preke", OwnerId = "owner-1" });
        await context.SaveChangesAsync();
        var service = new ReportService(context);
        await service.ReportItemAsync(1, "reporter-1", "Spamas");

        await Assert.ThrowsAsync<ModerationException>(() =>
            service.ReportItemAsync(1, "reporter-1", "Dar karta"));
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

    [Fact]
    public async Task GetReportedCommentsAsync_WhenActiveReportsExist_ReturnsGroupedCommentResponses()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        context.Items.Add(new Clothing { Id = 1, Name = "Striukė", OwnerId = "owner-1" });
        context.ItemComments.Add(new ItemComment { Id = 10, ItemId = 1, UserId = "author-1", Text = "Įžeidžiantis tekstas" });
        await context.SaveChangesAsync();
        var service = new ReportService(context);
        await service.ReportCommentAsync(10, "reporter-1", "Netinkamas komentaras");

        var reportedComments = await service.GetReportedCommentsAsync();

        Assert.Single(reportedComments);
        var dto = reportedComments[0];
        Assert.Equal(10, dto.CommentId);
        Assert.Equal(1, dto.ItemId);
        Assert.Equal("Striukė", dto.ItemName);
        Assert.Equal("Įžeidžiantis tekstas", dto.CommentText);
        Assert.Equal(1, dto.ReportCount);
        Assert.Single(dto.Reports);
        Assert.Equal("Netinkamas komentaras", dto.Reports[0].Reason);
        Assert.Equal("Active", dto.Reports[0].Status);
    }

    [Fact]
    public async Task DismissAsync_ActiveReport_MarksReportAsDismissed()
    {
        await using var context = TestHelpers.CreateInMemoryContext();
        context.Items.Add(new Clothing { Id = 1, Name = "Preke", OwnerId = "owner-1" });
        await context.SaveChangesAsync();
        var service = new ReportService(context);
        await service.ReportItemAsync(1, "reporter-1", "Spamas");
        var reportId = (await service.GetReportedItemsAsync())[0].Reports[0].Id;

        var dismissed = await service.DismissAsync(reportId);

        Assert.True(dismissed);
        Assert.Empty(await service.GetReportedItemsAsync());
    }
}