using System.Collections.Generic;
using Model;

namespace BLL.Interfaces
{
    public interface ICuaHangBLL
    {
        List<CuaHangModel> GetAll();
        CuaHangModel GetById(int id);
        bool Create(CuaHangModel model);
        bool Update(CuaHangModel model);
        bool Delete(int id);
    }
}
