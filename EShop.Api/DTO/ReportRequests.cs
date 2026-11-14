namespace EShop.Api.DTO;

public sealed record CreateReportRequest(string Reason);

public sealed record ReportedItemResponse(
    int ItemId,
    string Name,
    string? Description,
    string? OwnerId,
    int ReportCount,
    IReadOnlyList<ReportResponse> Reports);

public sealed record ReportResponse(
    int Id,
    int ItemId,
    string Reason,
    DateTime CreatedAt,
    string Status);
