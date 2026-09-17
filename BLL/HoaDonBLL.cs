using System.Collections.Generic;
using BLL.Interfaces;
using DAL.Interfaces;
using Model;

namespace BLL
{
    public class HoaDonBLL : IHoaDonBLL
    {
        private readonly IHoaDonRepository _res;

        public HoaDonBLL(IHoaDonRepository res)
        {
            _res = res;
        }

        public List<HoaDonModel> GetAll()
        {
            return _res.GetAll();
        }

        public HoaDonModel GetById(int id)
        {
            return _res.GetById(id);
        }

        public bool Create(HoaDonModel model)
        {
            return _res.Create(model);
        }

        public bool Update(HoaDonModel model)
        {
            return _res.Update(model);
        }

        public bool Delete(int id)
        {
            return _res.Delete(id);
        }
    }
}
