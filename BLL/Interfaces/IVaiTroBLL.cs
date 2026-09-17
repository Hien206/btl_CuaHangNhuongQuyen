using System.Collections.Generic;
using Model;

namespace BLL.Interfaces
{
    public interface IVaiTroBLL
    {
        List<VaiTroModel> GetAll();
        VaiTroModel GetById(int id);
        bool Create(VaiTroModel model);
        bool Update(VaiTroModel model);
        bool Delete(int id);
    }
}
