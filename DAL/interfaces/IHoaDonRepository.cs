using System.Collections.Generic;
using Model;

namespace DAL.Interfaces
{
    public interface IHoaDonRepository
    {
        List<HoaDonModel> GetAll();
        HoaDonModel GetById(int id);
        bool Create(HoaDonModel model);
        bool Update(HoaDonModel model);
        bool Delete(int id);
    }
}
