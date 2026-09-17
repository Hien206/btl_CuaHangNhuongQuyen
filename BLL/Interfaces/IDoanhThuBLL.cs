using System.Collections.Generic;
using Model;

namespace BLL.Interfaces
{
    public interface IDoanhThuBLL
    {
        List<DoanhThuModel> GetAll();
        DoanhThuModel GetById(int id);
        bool Create(DoanhThuModel model);
        bool Update(DoanhThuModel model);
        bool Delete(int id);
    }
}
