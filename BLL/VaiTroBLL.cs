using System.Collections.Generic;
using BLL.Interfaces;
using DAL.Interfaces;
using Model;

namespace BLL
{
    public class VaiTroBLL : IVaiTroBLL
    {
        private readonly IVaiTroRepository _res;

        public VaiTroBLL(IVaiTroRepository res)
        {
            _res = res;
        }

        public List<VaiTroModel> GetAll()
        {
            return _res.GetAll();
        }

        public VaiTroModel GetById(int id)
        {
            return _res.GetById(id);
        }

        public bool Create(VaiTroModel model)
        {
            return _res.Create(model);
        }

        public bool Update(VaiTroModel model)
        {
            return _res.Update(model);
        }

        public bool Delete(int id)
        {
            return _res.Delete(id);
        }
    }
}
