using EShop.Api.DTO;
using EShop.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EShop.Api.Services;

public class AdminService : IAdminService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    private const string AdminRole = "Admin";

    private static readonly string[] AllowedRoles =
    [
        "User",
        "Moderator",
        "Admin"
    ];

    public AdminService(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<List<AdminUserResponse>> GetUsersAsync()
    {
        var users = _userManager.Users.ToList();

        var result = new List<AdminUserResponse>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            result.Add(new AdminUserResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles.ToList(),
                IsBlocked = user.LockoutEnd.HasValue &&
                            user.LockoutEnd.Value > DateTimeOffset.UtcNow
            });
        }

        return result;
    }

    public async Task<bool> BlockUserAsync(
        string userId,
        string currentAdminId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return false;
        }

        if (user.Id == currentAdminId)
        {
            throw new InvalidOperationException(
                "Negalite užblokuoti savo paskyros.");
        }

        if (await _userManager.IsInRoleAsync(user, AdminRole))
        {
            throw new InvalidOperationException(
                "Negalima užblokuoti administratoriaus.");
        }

        var enableLockout = await _userManager
            .SetLockoutEnabledAsync(user, true);

        if (!enableLockout.Succeeded)
        {
            throw new InvalidOperationException(
                "Nepavyko įjungti naudotojo blokavimo.");
        }

        var result = await _userManager.SetLockoutEndDateAsync(
            user,
            DateTimeOffset.MaxValue);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ",
                    result.Errors.Select(error => error.Description)));
        }

        return true;
    }

    public async Task<bool> UnblockUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return false;
        }

        var result = await _userManager.SetLockoutEndDateAsync(
            user,
            null);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ",
                    result.Errors.Select(error => error.Description)));
        }

        return true;
    }

    public async Task<bool> ChangeUserRoleAsync(
        string userId,
        string newRole,
        string currentAdminId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return false;
        }

        if (user.Id == currentAdminId)
        {
            throw new InvalidOperationException(
                "Negalite keisti savo rolės.");
        }

        if (!AllowedRoles.Contains(
                newRole,
                StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Nurodyta netinkama rolė.");
        }

        var role = AllowedRoles.First(
            r => r.Equals(newRole, StringComparison.OrdinalIgnoreCase));

        if (!await _roleManager.RoleExistsAsync(role))
        {
            throw new InvalidOperationException(
                "Nurodyta rolė neegzistuoja.");
        }

        var currentRoles = await _userManager.GetRolesAsync(user);

        if (currentRoles.Contains(AdminRole) && role != AdminRole)
        {
            var admins = await _userManager.GetUsersInRoleAsync(AdminRole);

            if (admins.Count <= 1)
            {
                throw new InvalidOperationException(
                    "Negalima pašalinti paskutinio administratoriaus.");
            }
        }

        var removeResult = await _userManager.RemoveFromRolesAsync(
            user,
            currentRoles);

        if (!removeResult.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ",
                    removeResult.Errors.Select(error => error.Description)));
        }

        var addResult = await _userManager.AddToRoleAsync(user, role);

        if (!addResult.Succeeded)
        {
            if (currentRoles.Count > 0)
            {
                await _userManager.AddToRolesAsync(user, currentRoles);
            }

            throw new InvalidOperationException(
                string.Join(", ",
                    addResult.Errors.Select(error => error.Description)));
        }

        return true;
    }
}