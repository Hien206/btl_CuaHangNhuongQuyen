using System.Collections.Generic;
using BLL.Interfaces;
using DAL.Interfaces;
using Model;

namespace BLL
{
    public class DoanhThuBLL : IDoanhThuBLL
    {
        private readonly IDoanhThuRepository _res;

        public DoanhThuBLL(IDoanhThuRepository res)
        {
            _res = res;
        }

        public List<DoanhThuModel> GetAll()
        {
            return _res.GetAll();
        }

        public DoanhThuModel GetById(int id)
        {
            return _res.GetById(id);
        }

        public bool Create(DoanhThuModel model)
        {
            return _res.Create(model);
        }

        public bool Update(DoanhThuModel model)
        {
            return _res.Update(model);
        }

        public bool Delete(int id)
        {
            return _res.Delete(id);
        }
    }
}
