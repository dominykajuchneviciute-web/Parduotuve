using EShop.Api.DTO;

namespace EShop.Api.Services;

public interface IReportService
{
    Task ReportItemAsync(int itemId, string reporterId, string reason);
    Task<List<ReportedItemResponse>> GetReportedItemsAsync();
    Task<bool> DismissAsync(int reportId);
}
