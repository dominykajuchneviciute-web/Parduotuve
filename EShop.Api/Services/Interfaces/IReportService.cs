using EShop.Api.DTO;

namespace EShop.Api.Services.Interfaces;

public interface IReportService
{
    Task ReportItemAsync(int itemId, string reporterId, string reason);
    Task ReportCommentAsync(int commentId, string reporterId, string reason);
    Task<List<ReportedItemResponse>> GetReportedItemsAsync();
    Task<List<ReportedCommentResponse>> GetReportedCommentsAsync();
    Task<bool> DismissAsync(int reportId);
}