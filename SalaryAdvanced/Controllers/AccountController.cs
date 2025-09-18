using Microsoft.AspNetCore.Mvc;
using SalaryAdvanced.Application.Interfaces;
using SalaryAdvanced.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace SalaryAdvanced.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthenticationService authService, ILogger<AccountController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
                }

                var result = await _authService.SignInAsync(loginDto.Email, loginDto.Password);
                
                if (result)
                {
                    var currentUser = await _authService.GetCurrentUserAsync();
                    return Ok(new 
                    { 
                        message = "Đăng nhập thành công",
                        user = new
                        {
                            currentUser?.FullName,
                            currentUser?.Email,
                            currentUser?.EmployeeCode
                        }
                    });
                }

                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đăng nhập với email: {Email}", loginDto.Email);
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình đăng nhập" });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authService.SignOutAsync();
                return Ok(new { message = "Đăng xuất thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đăng xuất");
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình đăng xuất" });
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var currentUser = await _authService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return Unauthorized(new { message = "Không tìm thấy thông tin người dùng" });
                }

                return Ok(new
                {
                    currentUser.Id,
                    currentUser.FullName,
                    currentUser.Email,
                    currentUser.EmployeeCode,
                    Department = currentUser.Department?.Name,
                    currentUser.BasicSalary,
                    currentUser.HireDate
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin profile");
                return StatusCode(500, new { message = "Có lỗi xảy ra khi lấy thông tin người dùng" });
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
                }

                var currentUser = await _authService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return Unauthorized(new { message = "Không tìm thấy thông tin người dùng" });
                }

                var result = await _authService.ChangePasswordAsync(currentUser, 
                    changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

                if (result)
                {
                    return Ok(new { message = "Đổi mật khẩu thành công" });
                }

                return BadRequest(new { message = "Mật khẩu hiện tại không đúng" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đổi mật khẩu");
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình đổi mật khẩu" });
            }
        }
    }
}