using EShop.Api.Enum;

namespace EShop.Api.Models;

public class ItemReport
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public string ReporterId { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public ReportStatus Status { get; set; } = ReportStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAt { get; set; }
}
