﻿using Microsoft.AspNetCore.Mvc;
using QLSanBong_API.Models;
using QLSanBong_API.Services.IService;

namespace QLSanBong_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IKhachHangService _khachHangService;

        public AccountController(ILoginService loginService,IKhachHangService khachHangService)
        {
            _loginService = loginService;
            _khachHangService = khachHangService;
        }

        [HttpPost]
        [Route("login")]
        public ActionResult<string> Login(LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Thiếu thông tin tên đăng nhập hoặc mật khẩu.");
            }

            try
            {
                var token = _loginService.Login(request.Username, request.Password);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message }); // Trả về thông báo chi tiết cho client
            }
            catch (Exception)
            {
                return StatusCode(500, "Có lỗi xảy ra. Vui lòng thử lại.");
            }
        }

        [HttpPost("signup")]
        public ActionResult<KhachHang> Signup(KhachHangVM khachHangVM)
        {
            // Kiểm tra xem khachHangVM có null hay không và các trường cần thiết
            if (khachHangVM == null ||
                string.IsNullOrEmpty(khachHangVM.TenKh) ||  // Kiểm tra tên khách hàng
                string.IsNullOrEmpty(khachHangVM.Sdt) ||   // Kiểm tra số điện thoại
                string.IsNullOrEmpty(khachHangVM.Tendangnhap) ||
                string.IsNullOrEmpty(khachHangVM.TaiKhoan?.Password))
            {
                return BadRequest("Thiếu thông tin đăng ký.");
            }

            try
            {
                // Kiểm tra xem tên đăng nhập đã tồn tại
                if (_loginService.IsUsernameTaken(khachHangVM.Tendangnhap))
                {
                    return BadRequest("Tài khoản đã tồn tại.");
                }

                // Kiểm tra xem số điện thoại đã tồn tại
                if (_loginService.IsSdtTaken(khachHangVM.Sdt))
                {
                    return BadRequest("Số điện thoại đã tồn tại.");
                }

                // Thêm khách hàng
                _khachHangService.Add(khachHangVM);

                // Trả về kết quả thành công
                return CreatedAtAction(nameof(Signup), new { id = khachHangVM.Tendangnhap }, khachHangVM);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Có lỗi xảy ra khi đăng ký. Vui lòng thử lại.");
            }
        }


    }
}
