using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLSanBong_API.Models;
using QLSanBong_API.Services.IService;

namespace QLSanBong_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanBongController : ControllerBase
    {
        private readonly ISanBongService _sanBongService;

        public SanBongController(ISanBongService sanBongService)
        {
            _sanBongService = sanBongService;
        }

        // Lấy tất cả sân bóng
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var sanBongs = _sanBongService.GetAll();
            return Ok(sanBongs);
        }

        // Lấy sân bóng theo ID
        [HttpGet("getbyid")]
        public IActionResult GetById([FromQuery] string id)
        {
            var sanBong = _sanBongService.GetById(id);
            if (sanBong == null)
            {
                return NotFound();
            }
            return Ok(sanBong);
        }

        // Thêm sân bóng
        [HttpPost("add")]
        [Authorize(Policy = "RequireAdminRole")] // Chỉ cho phép admin thực hiện
        public IActionResult Add([FromForm] SanBongVM sanBongVM, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _sanBongService.Add(sanBongVM, imageFile);
            return CreatedAtAction(nameof(GetById), new { id = sanBongVM.TenSb }, sanBongVM);
        }

        // Cập nhật sân bóng
        [HttpPut("update")]
        [Authorize(Policy = "RequireAdminRole")] // Chỉ cho phép admin thực hiện
        public IActionResult Update([FromQuery] string id, [FromForm] SanBongVM sanBongVM, IFormFile? imageFile) // Đảm bảo `imageFile` có thể là null
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ", errors = ModelState });
            }

            try
            {
                _sanBongService.Update(id, sanBongVM, imageFile);
                return Ok(new { message = "Cập nhật sân bóng thành công" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Sân bóng không tồn tại" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Cập nhật thất bại", error = ex.Message });
            }
        }


        // Xóa sân bóng
        [HttpDelete("delete")]
        [Authorize(Policy = "RequireAdminRole")] // Chỉ cho phép admin thực hiện
        public IActionResult Delete([FromQuery]string id)
        {
            try
            {
                _sanBongService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
