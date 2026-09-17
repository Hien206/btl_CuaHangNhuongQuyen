using System.Collections.Generic;
using Model;

namespace BLL.Interfaces
{
    public interface IKhuyenMaiBLL
    {
        List<KhuyenMaiModel> GetAll();
        KhuyenMaiModel GetById(int id);
        bool Create(KhuyenMaiModel model);
        bool Update(KhuyenMaiModel model);
        bool Delete(int id);
    }
}
