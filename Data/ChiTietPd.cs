using System;
using System.Collections.Generic;

namespace QLSanBong_API.Data;

public partial class ChiTietPd
{
    public string MaPds { get; set; } = null!;

    public string MaGio { get; set; } = null!;

    public DateOnly? Ngaysudung { get; set; }

    public decimal? Giatien { get; set; }

    public string? Ghichu { get; set; }

    public TimeOnly? Giobatdau { get; set; }

    public TimeOnly? Gioketthuc { get; set; }

    public virtual GiaGioThue MaGioNavigation { get; set; } = null!;

    public virtual PhieuDatSan MaPdsNavigation { get; set; } = null!;
}
