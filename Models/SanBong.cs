using System;
using System.Collections.Generic;

namespace QLSanBong_API.Models
{
    public class SanBongVM
    {

        public string TenSb { get; set; } = null!;

        public string? Dientich { get; set; }

        public string? Ghichu { get; set; }

        public string? Hinhanh { get; set; }
        public string? DiaChi { get; set; }

    }
    public partial class SanBong: SanBongVM
    {
        public string MaSb { get; set; } = null!;
    }
}