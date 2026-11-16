using EShop.Api.Data;
using EShop.Api.DTO;
using EShop.Api.Enum;
using EShop.Api.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EShop.Api.Services;

public sealed class ReportService : IReportService
{
    private readonly AppDbContext _context;

    public ReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task ReportItemAsync(int itemId, string reporterId, string reason)
    {
        if (!await _context.Items.AnyAsync(item => item.Id == itemId))
        {
            throw new KeyNotFoundException("Skelbimas nerastas.");
        }

        if (await _context.ItemReports.AnyAsync(report =>
                report.ItemId == itemId &&
                report.ReporterId == reporterId &&
                report.Status == ReportStatus.Active))
        {
            throw new ModerationException("Šį skelbimą jau pranešėte.");
        }

        _context.ItemReports.Add(new ItemReport
        {
            ItemId = itemId,
            ReporterId = reporterId,
            Reason = reason.Trim()
        });

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException postgresException &&
                                                  postgresException.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new ModerationException("Šį skelbimą jau pranešėte.");
        }
    }

    public async Task ReportCommentAsync(int commentId, string reporterId, string reason)
    {
        var comment = await _context.ItemComments
            .FirstOrDefaultAsync(comment => comment.Id == commentId);

        if (comment is null)
        {
            throw new KeyNotFoundException("Komentaras nerastas.");
        }

        if (comment.UserId == reporterId)
        {
            throw new ModerationException("Negalite pranešti apie savo komentarą.");
        }

        if (await _context.ItemReports.AnyAsync(report =>
                report.CommentId == commentId &&
                report.ReporterId == reporterId &&
                report.Status == ReportStatus.Active))
        {
            throw new ModerationException("Apie šį komentarą jau pranešėte.");
        }

        _context.ItemReports.Add(new ItemReport
        {
            CommentId = commentId,
            ReporterId = reporterId,
            Reason = reason.Trim()
        });

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException postgresException &&
                                                  postgresException.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new ModerationException("Apie šį komentarą jau pranešėte.");
        }
    }

    public async Task<List<ReportedItemResponse>> GetReportedItemsAsync()
    {
        var reports = await _context.ItemReports
            .Where(report => report.Status == ReportStatus.Active && report.ItemId.HasValue)
            .OrderByDescending(report => report.CreatedAt)
            .ToListAsync();

        var itemIds = reports.Select(report => report.ItemId!.Value).Distinct().ToList();
        var items = await _context.Items
            .Where(item => itemIds.Contains(item.Id))
            .ToDictionaryAsync(item => item.Id);

        return reports
            .GroupBy(report => report.ItemId!.Value)
            .Where(group => items.ContainsKey(group.Key))
            .Select(group =>
            {
                var item = items[group.Key];
                return new ReportedItemResponse(
                    item.Id,
                    item.Name,
                    item.Description,
                    item.OwnerId,
                    group.Count(),
                    group.Select(report => new ReportResponse(
                        report.Id,
                        report.ItemId,
                        report.Reason,
                        report.CreatedAt,
                        report.Status.ToString())).ToList());
            })
            .ToList();
    }

    public async Task<List<ReportedCommentResponse>> GetReportedCommentsAsync()
    {
        var reports = await _context.ItemReports
            .Where(report => report.Status == ReportStatus.Active && report.CommentId.HasValue)
            .OrderByDescending(report => report.CreatedAt)
            .ToListAsync();

        var commentIds = reports.Select(report => report.CommentId!.Value).Distinct().ToList();
        var comments = await _context.ItemComments
            .Where(comment => commentIds.Contains(comment.Id))
            .ToDictionaryAsync(comment => comment.Id);

        var itemIds = comments.Values.Select(comment => comment.ItemId).Distinct().ToList();
        var items = await _context.Items
            .Where(item => itemIds.Contains(item.Id))
            .ToDictionaryAsync(item => item.Id);

        return reports
            .GroupBy(report => report.CommentId!.Value)
            .Where(group => comments.ContainsKey(group.Key) &&
                            items.ContainsKey(comments[group.Key].ItemId))
            .Select(group =>
            {
                var comment = comments[group.Key];
                var item = items[comment.ItemId];
                return new ReportedCommentResponse(
                    comment.Id,
                    item.Id,
                    item.Name,
                    comment.Text,
                    group.Count(),
                    group.Select(report => new ReportResponse(
                        report.Id,
                        report.ItemId,
                        report.Reason,
                        report.CreatedAt,
                        report.Status.ToString())).ToList());
            })
            .ToList();
    }

    public async Task<bool> DismissAsync(int reportId)
    {
        var report = await _context.ItemReports
            .FirstOrDefaultAsync(report => report.Id == reportId && report.Status == ReportStatus.Active);

        if (report is null)
        {
            return false;
        }

        report.Status = ReportStatus.Dismissed;
        report.ResolvedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}
