using System.Collections.Generic;
using Model;

namespace BLL.Interfaces
{
    public interface IHoaDonBLL
    {
        List<HoaDonModel> GetAll();
        HoaDonModel GetById(int id);
        bool Create(HoaDonModel model);
        bool Update(HoaDonModel model);
        bool Delete(int id);
    }
}
