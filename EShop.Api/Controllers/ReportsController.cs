using System.Security.Claims;
using EShop.Api.DTO;
using EShop.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers;

[ApiController]
[Route("api/reports")]
public sealed class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [Authorize]
    [HttpPost("/api/items/{itemId:int}/reports")]
    public async Task<IActionResult> ReportItem(int itemId, CreateReportRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Reason))
        {
            return BadRequest("Pranešimo priežastis yra privaloma.");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        try
        {
            await _reportService.ReportItemAsync(itemId, userId, request.Reason);
            return Ok();
        }
        catch (ModerationException exception)
        {
            return Conflict(exception.Message);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [Authorize(Roles = "Moderator")]
    [HttpGet]
    public async Task<ActionResult<List<ReportedItemResponse>>> GetReportedItems()
    {
        return Ok(await _reportService.GetReportedItemsAsync());
    }

    [Authorize(Roles = "Moderator")]
    [HttpPost("{reportId:int}/dismiss")]
    public async Task<IActionResult> DismissReport(int reportId)
    {
        return await _reportService.DismissAsync(reportId)
            ? NoContent()
            : NotFound("Pranešimas nerastas arba jau išspręstas.");
    }
}
