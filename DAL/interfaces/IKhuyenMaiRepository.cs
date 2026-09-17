using System.Collections.Generic;
using Model;

namespace DAL.Interfaces
{
    public interface IKhuyenMaiRepository
    {
        List<KhuyenMaiModel> GetAll();
        KhuyenMaiModel GetById(int id);
        bool Create(KhuyenMaiModel model);
        bool Update(KhuyenMaiModel model);
        bool Delete(int id);
    }
}
