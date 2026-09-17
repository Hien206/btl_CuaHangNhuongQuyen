using System.Collections.Generic;
using Model;

namespace DAL.Interfaces
{
    public interface ICuaHangRepository
    {
        List<CuaHangModel> GetAll();
        CuaHangModel GetById(int id);
        bool Create(CuaHangModel model);
        bool Update(CuaHangModel model);
        bool Delete(int id);
    }
}
