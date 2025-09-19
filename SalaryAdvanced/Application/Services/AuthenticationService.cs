using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using SalaryAdvanced.Application.Interfaces;
using SalaryAdvanced.Domain.Entities;
using System.Security.Claims;
using SalaryAdvanced.Application.DTOs;

namespace SalaryAdvanced.Application.Services
{
    public class AuthenticationService : Interfaces.IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            AuthenticationStateProvider authStateProvider,
            ILogger<AuthenticationService> logger)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _authStateProvider = authStateProvider;
            _logger = logger;
        }

        public async Task<bool> SignInAsync(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null || !user.IsActive)
                    return false;
                var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
                if (!isValidPassword)
                    return false;

                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    throw new InvalidOperationException("HttpContext is not available.");
                }

                var principal = await CreateClaimsPrincipalAsync(user);
                await httpContext.SignInAsync(
                    IdentityConstants.ApplicationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                    });
                if (_authStateProvider is Infrastructure.Auth.CustomAuthenticationStateProvider customProvider)
                {
                    customProvider.NotifyUserAuthentication(principal);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when login");
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task SignOutAsync()
        {
            try
            {
                if (_authStateProvider is Infrastructure.Auth.CustomAuthenticationStateProvider customProvider)
                {
                    customProvider.NotifyUserLogout();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when logout");
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ApplicationUser?> GetCurrentUserAsync()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext?.User?.Identity?.IsAuthenticated == true)
                {
                    var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (int.TryParse(userId, out var id))
                    {
                        return await _userManager.Users
                            .Include(u => u.Department)
                            .FirstOrDefaultAsync(u => u.Id == id);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current user");
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string role)
        {
            try
            {
                return await _userManager.IsInRoleAsync(user, role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking user role for user {UserId}, role {Role}", user?.Id, role);
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> RegisterAsync(ApplicationUser user, string password)
        {
            try
            {
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employee");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user {Email}", user?.Email);
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            try
            {
                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user {UserId}", user?.Id);
                throw new Exception(ex.Message, ex);
            }
        }

        private async Task<ClaimsPrincipal> CreateClaimsPrincipalAsync(ApplicationUser user)
        {
            try
            {
                var roles = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.UserName ?? ""),
                    new(ClaimTypes.Email, user.Email ?? ""),
                    new("FullName", user.FullName),
                    new("EmployeeCode", user.EmployeeCode)
                };
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(
                    claims,
                    IdentityConstants.ApplicationScheme);

                return new ClaimsPrincipal(identity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating claims principal for user {UserId}", user?.Id);
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<UserProfileDto?> GetUserProfileAsync()
        {
            try
            {
                var currentUser = await GetCurrentUserAsync();
                if (currentUser == null)
                    return null;

                var roles = await _userManager.GetRolesAsync(currentUser);
                var role = roles.FirstOrDefault() ?? "Employee";

                return new UserProfileDto
                {
                    Id = currentUser.Id,
                    EmployeeCode = currentUser.EmployeeCode,
                    UserName = currentUser.UserName ?? "",
                    Email = currentUser.Email ?? "",
                    FullName = currentUser.FullName,
                    Phone = currentUser.PhoneNumber,
                    BasicSalary = currentUser.BasicSalary,
                    HireDate = currentUser.HireDate,
                    DepartmentId = currentUser.DepartmentId,
                    DepartmentName = currentUser.Department?.Name ?? "",
                    Role = role,
                    IsActive = currentUser.IsActive
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile");
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateUserProfileAsync(UpdateUserProfileDto updateDto)
        {
            try
            {
                var currentUser = await GetCurrentUserAsync();
                if (currentUser == null)
                    return false;

                currentUser.FullName = updateDto.FullName;
                currentUser.PhoneNumber = updateDto.Phone;

                var result = await _userManager.UpdateAsync(currentUser);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user profile");
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            try
            {
                var currentUser = await GetCurrentUserAsync();
                if (currentUser == null)
                    return false;

                return await ChangePasswordAsync(currentUser, currentPassword, newPassword);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in change password wrapper method");
                throw new Exception(ex.Message, ex);
            }
        }
    }
}