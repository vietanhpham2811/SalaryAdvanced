using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using SalaryAdvanced.Application.Interfaces;
using SalaryAdvanced.Domain.Entities;
using System.Security.Claims;

namespace SalaryAdvanced.Application.Services
{
    public class AuthenticationService : SalaryAdvanced.Application.Interfaces.IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            AuthenticationStateProvider authStateProvider)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> SignInAsync(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email) ?? 
                          await _userManager.FindByNameAsync(email);

                if (user == null || !user.IsActive)
                    return false;
                var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
                if (!isValidPassword)
                    return false;

                var httpContext = _httpContextAccessor.HttpContext;

                if (httpContext == null)
                {

                    throw new InvalidOperationException("HttpContext is not available. This can happen in Blazor Server during SignalR calls. Try calling this method from a component that has HTTP context.");
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

                // Notify Blazor authentication state changed
                if (_authStateProvider is Infrastructure.Auth.CustomAuthenticationStateProvider customProvider)
                {
                    customProvider.NotifyUserAuthentication(principal);
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log the actual exception for debugging
                System.Diagnostics.Debug.WriteLine($"SignIn error: {ex.Message}");
                return false;
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
                System.Diagnostics.Debug.WriteLine($"SignOut error: {ex.Message}");
                if (_authStateProvider is Infrastructure.Auth.CustomAuthenticationStateProvider customProvider)
                {
                    customProvider.NotifyUserLogout();
                }
            }
        }

        public async Task<ApplicationUser?> GetCurrentUserAsync()
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

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> RegisterAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Employee");
                
                return true;
            }
            
            return false;
        }

        public async Task<bool> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }

        private async Task<ClaimsPrincipal> CreateClaimsPrincipalAsync(ApplicationUser user)
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

            // Add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(
                claims, 
                IdentityConstants.ApplicationScheme);

            return new ClaimsPrincipal(identity);
        }
    }
}