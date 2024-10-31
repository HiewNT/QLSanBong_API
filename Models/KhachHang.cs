﻿using System;
using System.Collections.Generic;

namespace QLSanBong_API.Models
{

    public class KhachHangVM
    {

        public string TenKh { get; set; } = null!;

        public string Sdt { get; set; } = null!;

        public string? Gioitinh { get; set; }

        public string? Diachi { get; set; }
        public string Tendangnhap { get; set; } = null!;
        public virtual TaiKhoanVM? TaiKhoan { get; set; }
    }
    public partial class KhachHang:KhachHangVM {
        public string MaKh { get; set; } = null!;
    }
    public class KhachHangDS
    {
        public string TenKh { get; set; } = null!;

        public string Sdt { get; set; } = null!;
        public string? Gioitinh { get; set; }

        public string? Diachi { get; set; }
    }
}
