using QLSanBong_API.Models;

namespace QLSanBong_API.Services.IService
{
    public interface IYeuCauDatSanService
    {
        IEnumerable<YeuCauDatSan> GetAll();

        YeuCauDatSan GetByID(int id);

        YeuCauDatSan GetBy(GetYCDSRequest request);

        void Add(YeuCauDatSanAdd YeuCauDatSanAdd);

        void Update(UpdateYCDSRequest request);

        void Delete(int id);
    }
}
