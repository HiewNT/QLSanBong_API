using QLSanBong_API.Models;

namespace QLSanBong_API.Services.IService
{
    public interface IPhieuDatSanService
    {
        IEnumerable<PhieuDatSan> GetAll();

        PhieuDatSan GetById(string id);

        void Add(PhieuDatSanVM phieuDatSanVM);

        void Update(string id, PhieuDatSanVM phieuDatSanVM);

        void Delete(string id);
    }
}
