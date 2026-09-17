using System.Collections.Generic;
using Model;

namespace BLL.Interfaces
{
    public interface IHopDongBLL
    {
        List<HopDongModel> GetAll();
        HopDongModel GetById(int id);
        bool Create(HopDongModel model);
        bool Update(HopDongModel model);
        bool Delete(int id);
    }
}
