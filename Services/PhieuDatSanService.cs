using Microsoft.AspNetCore.Http.HttpResults;
using QLSanBong_API.Data; // Namespace chứa DbContext và các lớp Data
using QLSanBong_API.Models; // Namespace chứa các lớp Models
using QLSanBong_API.Services.IService;
using System.Collections.Generic; // Namespace cho List
using System.Linq; // Namespace cho LINQ

namespace QLSanBong_API.Services
{
    public class PhieuDatSanService : IPhieuDatSanService
    {
        private readonly QlsanBongContext _context;

        public PhieuDatSanService(QlsanBongContext context)
        {
            _context = context;
        }

        public IEnumerable<Models.PhieuDatSan> GetAll()
        {
            // Lấy tất cả phiếu đặt sân từ cơ sở dữ liệu
            var pdsList = _context.PhieuDatSans.ToList();

            var pdsData=pdsList.Select(pds=>new Models.PhieuDatSan
            { 
                    MaPds = pds.MaPds,
                    Ngaylap = pds.Ngaylap != null ? pds.Ngaylap.Value.ToString("yyyy-MM-dd") : null, // Chuyển đổi DateTime thành string với định dạng
                    TongTien = pds.TongTien.HasValue ? pds.TongTien.Value.ToString("N0") : null, // Chuyển đổi decimal thành string
                    Phuongthuctt = pds.Phuongthuctt,
                    GhiChu = pds.GhiChu,
                    Sttds = pds.Sttds,
                    MaNv = pds.MaNv,
                    MaKh = pds.MaKh
                }).ToList();

            return pdsData;
        }

        public Models.PhieuDatSan GetById(string id)
        {
            var phieuDatSan = _context.PhieuDatSans.SingleOrDefault(pds => pds.MaPds == id);
            if (phieuDatSan == null)
            {
                return null;
            }

            return new Models.PhieuDatSan
            {
                MaPds = phieuDatSan.MaPds,
                Ngaylap = phieuDatSan.Ngaylap?.ToString("yyyy-MM-dd"),
                TongTien = phieuDatSan.TongTien.HasValue ? phieuDatSan.TongTien.Value.ToString("N0") : null,
                Phuongthuctt = phieuDatSan.Phuongthuctt,
                GhiChu = phieuDatSan.GhiChu,
                Sttds = phieuDatSan.Sttds,
                MaNv = phieuDatSan.MaNv,
                MaKh = phieuDatSan.MaKh
            };
        }

        public void Add(PhieuDatSanVM phieuDatSanVM)
        {
            var phieuDatSan = new Data.PhieuDatSan
            {
                MaPds = GenerateMaPds(), // Giả sử bạn có một phương thức để tạo mã phiếu đặt sân
                Ngaylap = DateTime.Parse(phieuDatSanVM.Ngaylap), // Chuyển đổi string thành DateTime
                TongTien = decimal.TryParse(phieuDatSanVM.TongTien, out var tongTien) ? (decimal?)tongTien : null,
                Phuongthuctt = phieuDatSanVM.Phuongthuctt,
                GhiChu = phieuDatSanVM.GhiChu,
                Sttds = phieuDatSanVM.Sttds,
                MaNv = phieuDatSanVM.MaNv,
                MaKh = phieuDatSanVM.MaKh
            };

            _context.PhieuDatSans.Add(phieuDatSan);
            _context.SaveChanges();
        }

        public void Update(string id, PhieuDatSanVM phieuDatSanVM)
        {
            var phieuDatSan = _context.PhieuDatSans.SingleOrDefault(pds => pds.MaPds == id);
            if (phieuDatSan == null)
            {
                throw new KeyNotFoundException("Phiếu đặt sân không tồn tại.");
            }

            phieuDatSan.Ngaylap = DateTime.Parse(phieuDatSanVM.Ngaylap);
            phieuDatSan.TongTien = decimal.TryParse(phieuDatSanVM.TongTien, out var tongTien) ? (decimal?)tongTien : null;
            phieuDatSan.Phuongthuctt = phieuDatSanVM.Phuongthuctt;
            phieuDatSan.GhiChu = phieuDatSanVM.GhiChu;
            phieuDatSan.Sttds = phieuDatSanVM.Sttds;
            phieuDatSan.MaNv = phieuDatSanVM.MaNv;
            phieuDatSan.MaKh = phieuDatSanVM.MaKh;

            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            var phieuDatSan = _context.PhieuDatSans.SingleOrDefault(pds => pds.MaPds == id);
            if (phieuDatSan == null)
            {
                throw new KeyNotFoundException("Phiếu đặt sân không tồn tại.");
            }

            _context.PhieuDatSans.Remove(phieuDatSan);
            _context.SaveChanges();
        }

        private string GenerateMaPds()
        {
            string newMaPds = "PDS00001"; // Mã mặc định nếu chưa có sân bóng nào

            if (_context.PhieuDatSans.Any())
            {
                var maxMaPds = _context.PhieuDatSans
                    .Where(m => m.MaPds.StartsWith("PDS"))
                    .OrderByDescending(m => m.MaPds)
                    .Select(m => m.MaPds)
                    .FirstOrDefault();

                if (maxMaPds != null)
                {
                    int currentNumber = int.Parse(maxMaPds.Substring(3));
                    newMaPds = $"PDS{(currentNumber + 1).ToString("D5")}";
                }
            }

            return newMaPds;
        }
    }
}
