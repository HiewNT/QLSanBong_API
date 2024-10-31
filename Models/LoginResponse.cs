namespace QLSanBong_API.Models
{
    public class LoginResponse
    {
        public string MaNv { get; set; }        // Mã nhân viên
        public string TenNv { get; set; }       // Tên nhân viên
        public string Chucvu { get; set; }      // Chức vụ
        public string Token { get; set; }       // JWT Token
    }
}
