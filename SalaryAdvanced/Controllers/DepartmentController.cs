using Microsoft.AspNetCore.Mvc;
using SalaryAdvanced.Application.Interfaces;
using SalaryAdvanced.Application.DTOs;

namespace SalaryAdvanced.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _service;
        private readonly ILogger<DepartmentController> _logger;
        public DepartmentController(IDepartmentService service, ILogger<DepartmentController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Get() 
        { 
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
                }
                var departments = await _service.GetAllDepartmentsAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách phòng ban");
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình lấy danh sách phòng ban" });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
                }
                var department = await _service.GetDepartmentByIdAsync(id);
                if (department == null)
                {
                    return NotFound(new { message = "Phòng ban không tồn tại" });
                }
                return Ok(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin phòng ban với id: {Id}", id);
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình lấy thông tin phòng ban" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DepartmentDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
                }
                var createdDepartment = await _service.AddDepartmentAsync(dto);
                if (createdDepartment == null)
                {
                    return BadRequest(new { message = "Không thể tạo phòng ban" });
                }
                return CreatedAtAction(nameof(Get), new { id = createdDepartment.Id }, createdDepartment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo phòng ban");
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình tạo phòng ban" });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] DepartmentDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
                }
                var updatedDepartment = await _service.UpdateDepartmentAsync(id, dto);
                if (updatedDepartment == null)
                {
                    return NotFound(new { message = "Phòng ban không tồn tại" });
                }
                return Ok(updatedDepartment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật phòng ban với id: {Id}", id);
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình cập nhật phòng ban" });
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
                var result = await _service.DeleteDepartmentAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "Phòng ban không tồn tại" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa phòng ban với id: {Id}", id);
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình xóa phòng ban" });
            }
        }
    }
}
