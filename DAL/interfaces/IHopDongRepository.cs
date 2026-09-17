using System.Collections.Generic;
using Model;

namespace DAL.Interfaces
{
    public interface IHopDongRepository
    {
        List<HopDongModel> GetAll();
        HopDongModel GetById(int id);
        bool Create(HopDongModel model);
        bool Update(HopDongModel model);
        bool Delete(int id);
    }
}
