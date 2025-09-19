using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SalaryAdvanced.Application.DTOs;
using SalaryAdvanced.Application.Interfaces;

namespace SalaryAdvanced.Controllers
{
    [Route("api/salary-advance-requests")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly ISalaryAdvanceRequestService _service;
        private readonly ILogger<DepartmentController> _logger;
        public RequestController(ISalaryAdvanceRequestService service, ILogger<DepartmentController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
                }
                var request = await _service.GetSalaryAdvanceRequestsById(id);
                return Ok(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "lỗi xảy ra trong quá trình lấy đề xuất nâng lương");
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình lấy đề xuất nâng lương" });
            }
        }
        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
                }
                var list = await _service.GetSalaryAdvanceRequestsByEmployeeAsync(employeeId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách đề xuất nâng lương");
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình lấy đề xuất nâng lương" });
            }
        }
        [HttpGet("manager/{managerId}")]
        public async Task<IActionResult> GetByManagerId(int managerId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
                }
                var list = await _service.GetSalaryAdvanceRequestsForApprovalAsync(managerId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách đề xuất nâng lương");
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình lấy đề xuất nâng lương" });
            }
        }
        [HttpGet("status/{statusId}")]
        public async Task<IActionResult> GetByStatusId(int statusId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
                }
                var list = await _service.GetSalaryAdvanceRequestsByStatus(statusId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách đề xuất nâng lương");
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình lấy đề xuất nâng lương" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateRequestDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
                }
                var createdRequest = await _service.CreateSalaryAdvanceRequestAsync(dto);
                if (createdRequest == null)
                {
                    return BadRequest(new { message = "Không thể tạo đề xuất nâng lương" });
                }
                return CreatedAtAction(nameof(GetById), new { id = createdRequest.Id }, createdRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo đề xuất nâng lương");
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình tạo đề xuất nâng lương" });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
                }
                var result = await _service.DeleteSalaryAdvanceRequestAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "Đề xuất không tồn tại" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa đề xuất nâng lương");
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình xóa đề xuất nâng lương" });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]RequestResponseDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
                }
                var result = await _service.RespondToSalaryAdvanceRequestAsync(dto);
                if (result == null)
                {
                    return NotFound(new { message = "Đề xuất không tồn tại" });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa đề xuất nâng lương");
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình xóa đề xuất nâng lương" });
            }
        }
    }
}
