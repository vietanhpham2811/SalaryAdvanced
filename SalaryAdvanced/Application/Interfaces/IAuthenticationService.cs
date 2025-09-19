using SalaryAdvanced.Application.DTOs;
using SalaryAdvanced.Domain.Entities;

namespace SalaryAdvanced.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> SignInAsync(string email, string password);
        Task SignOutAsync();
        Task<ApplicationUser?> GetCurrentUserAsync();
        Task<UserProfileDto?> GetUserProfileAsync();
        Task<bool> UpdateUserProfileAsync(UpdateUserProfileDto updateDto);
        Task<bool> IsInRoleAsync(ApplicationUser user, string role);
        Task<bool> RegisterAsync(ApplicationUser user, string password);
        Task<bool> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
        Task<bool> ChangePasswordAsync(string currentPassword, string newPassword);
    }
}