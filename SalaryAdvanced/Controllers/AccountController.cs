using Microsoft.AspNetCore.Mvc;
using SalaryAdvanced.Application.Interfaces;
using SalaryAdvanced.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

namespace SalaryAdvanced.Controllers
{
    public class AccountController : Controller
    {
        private readonly Application.Interfaces.IAuthenticationService _authService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(SalaryAdvanced.Application.Interfaces.IAuthenticationService authService, ILogger<AccountController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromForm] string Email, [FromForm] string Password, [FromForm] string? ReturnUrl = null)
        {
            try
            {
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                {
                    return Redirect("/login?error=" + Uri.EscapeDataString("Email and password are required"));
                }
                var result = await _authService.SignInAsync(Email, Password);
                if (result)
                {
                    var returnUrl = ReturnUrl ?? "/";
                    return Redirect(returnUrl);
                }
                return Redirect("/login?error=" + Uri.EscapeDataString("Invalid email or password"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user: {Email}", Email);
                return Redirect("/login?error=" + Uri.EscapeDataString($"An error occurred: {ex.Message}"));
            }
        }

        [HttpGet("/logout")]
        public async Task<IActionResult> LogoutGet()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Redirect("/login");
        }
    }
}