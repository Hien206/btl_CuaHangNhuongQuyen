using System.Collections.Generic;
using BLL.Interfaces;
using DAL.Interfaces;
using Model;

namespace BLL
{
    public class CuaHangBLL : ICuaHangBLL
    {
        private readonly ICuaHangRepository _res;

        public CuaHangBLL(ICuaHangRepository res)
        {
            _res = res;
        }

        public List<CuaHangModel> GetAll()
        {
            return _res.GetAll();
        }

        public CuaHangModel GetById(int id)
        {
            return _res.GetById(id);
        }

        public bool Create(CuaHangModel model)
        {
            return _res.Create(model);
        }

        public bool Update(CuaHangModel model)
        {
            return _res.Update(model);
        }

        public bool Delete(int id)
        {
            return _res.Delete(id);
        }
    }
}
