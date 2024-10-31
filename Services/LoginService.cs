using QLSanBong_API.Data;
using QLSanBong_API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using QLSanBong_API.Services.IService;

namespace QLSanBong_API.Services
{
    public class LoginService : ILoginService
    {
        private readonly QlsanBongContext _context;
        private readonly IConfiguration _configuration;

        public LoginService(QlsanBongContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Phương thức xác thực người dùng và tạo token
        public string Login(string username, string password)
        {
            // Tìm tài khoản trong cơ sở dữ liệu
            var taiKhoanData = _context.TaiKhoans.SingleOrDefault(x => x.Username == username);

            // Kiểm tra tài khoản có tồn tại và mật khẩu có khớp không
            if (taiKhoanData == null || !VerifyPassword(password, taiKhoanData.Password))
            {
                throw new UnauthorizedAccessException("Tên đăng nhập hoặc mật khẩu không đúng");
            }

            // Chuyển đổi đối tượng từ Data.TaiKhoan sang Models.TaiKhoan
            var taiKhoan = new Models.TaiKhoan
            {
                Username = taiKhoanData.Username,
                Password = taiKhoanData.Password, // Có thể không cần lưu trữ mật khẩu
                Role = taiKhoanData.Role
            };

            // Tạo token cho người dùng đã xác thực
            return GenerateJwtToken(taiKhoan);
        }

        // Phương thức tạo JWT token
        public string GenerateJwtToken(Models.TaiKhoan taiKhoan) // Đảm bảo phương thức này là public
        {
            // Thông tin các claims để thêm vào token (như username và role)
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, taiKhoan.Username),
                new Claim(ClaimTypes.Role, taiKhoan.Role ?? "KhachHang") // Mặc định role là User nếu null
            };

            // Lấy khóa bí mật từ cấu hình
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tạo token với thời gian hết hạn và các claims
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60), // Token có thời hạn 30 phút
                signingCredentials: creds
            );

            // Trả về chuỗi token đã ký
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool IsUsernameTaken(string username)
        {
            // Kiểm tra xem tên người dùng đã tồn tại trong cơ sở dữ liệu chưa
            return _context.TaiKhoans.Any(x => x.Username == username);
        }
        public bool IsSdtTaken(string sdt)
        {
            // Kiểm tra xem tên người dùng đã tồn tại trong cơ sở dữ liệu chưa
            return _context.KhachHangs.Any(x => x.Sdt == sdt);
        }
        // Phương thức để kiểm tra mật khẩu có khớp với mật khẩu đã mã hóa hay không
        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
