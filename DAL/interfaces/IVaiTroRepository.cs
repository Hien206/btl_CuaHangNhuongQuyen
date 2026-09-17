using System.Collections.Generic;
using Model;

namespace DAL.Interfaces
{
    public interface IVaiTroRepository
    {
        List<VaiTroModel> GetAll();
        VaiTroModel GetById(int id);
        bool Create(VaiTroModel model);
        bool Update(VaiTroModel model);
        bool Delete(int id);
    }
}
