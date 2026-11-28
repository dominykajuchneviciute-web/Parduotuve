using EShop.Api.DTO;

namespace EShop.Api.Services.Interfaces;

public interface IAdminService
{
    Task<List<AdminUserResponse>> GetUsersAsync();

    Task<bool> BlockUserAsync(string userId, string currentAdminId);

    Task<bool> UnblockUserAsync(string userId);

    Task<bool> ChangeUserRoleAsync(
        string userId,
        string newRole,
        string currentAdminId);
}