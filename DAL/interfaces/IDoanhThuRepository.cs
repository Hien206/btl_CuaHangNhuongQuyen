using System.Collections.Generic;
using Model;

namespace DAL.Interfaces
{
    public interface IDoanhThuRepository
    {
        List<DoanhThuModel> GetAll();
        DoanhThuModel GetById(int id);
        bool Create(DoanhThuModel model);
        bool Update(DoanhThuModel model);
        bool Delete(int id);
    }
}
