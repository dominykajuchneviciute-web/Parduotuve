using System.Security.Claims;
using EShop.Api.DTO;
using EShop.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("users")]
    public async Task<ActionResult<List<AdminUserResponse>>> GetUsers()
    {
        var users = await _adminService.GetUsersAsync();

        return Ok(users);
    }

    [HttpPost("users/{userId}/block")]
    public async Task<IActionResult> BlockUser(string userId)
    {
        var currentAdminId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentAdminId))
        {
            return Unauthorized();
        }

        try
        {
            var blocked = await _adminService.BlockUserAsync(
                userId,
                currentAdminId);

            if (!blocked)
            {
                return NotFound("Naudotojas nerastas.");
            }

            return NoContent();
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost("users/{userId}/unblock")]
    public async Task<IActionResult> UnblockUser(string userId)
    {
        var unblocked = await _adminService.UnblockUserAsync(userId);

        if (!unblocked)
        {
            return NotFound("Naudotojas nerastas.");
        }

        return NoContent();
    }

    [HttpPut("users/{userId}/role")]
    public async Task<IActionResult> ChangeUserRole(
        string userId,
        ChangeUserRoleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NewRole))
        {
            return BadRequest("Nauja rolė yra privaloma.");
        }

        var currentAdminId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentAdminId))
        {
            return Unauthorized();
        }

        try
        {
            var changed = await _adminService.ChangeUserRoleAsync(
                userId,
                request.NewRole,
                currentAdminId);

            if (!changed)
            {
                return NotFound("Naudotojas nerastas.");
            }

            return NoContent();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}