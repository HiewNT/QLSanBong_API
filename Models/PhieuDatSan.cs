namespace QLSanBong_API.Models
{
    public class PhieuDatSanVM
    {
        public string? Ngaylap { get; set; }

        public string? TongTien { get; set; }

        public string? Phuongthuctt { get; set; }

        public string? GhiChu { get; set; }

        public int? Sttds { get; set; }

        public string MaNv { get; set; } = null!;

        public string MaKh { get; set; } = null!;

        public List<ChiTietPdsVM>? ChiTietPdsVM {  get; set; }
    }
    public partial class PhieuDatSan : PhieuDatSanVM
    {
        public string MaPds { get; set; } = null!;

    }
}
