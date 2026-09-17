using System.Collections.Generic;
using BLL.Interfaces;
using DAL.Interfaces;
using Model;

namespace BLL
{
    public class HopDongBLL : IHopDongBLL
    {
        private readonly IHopDongRepository _res;

        public HopDongBLL(IHopDongRepository res)
        {
            _res = res;
        }

        public List<HopDongModel> GetAll()
        {
            return _res.GetAll();
        }

        public HopDongModel GetById(int id)
        {
            return _res.GetById(id);
        }

        public bool Create(HopDongModel model)
        {
            return _res.Create(model);
        }

        public bool Update(HopDongModel model)
        {
            return _res.Update(model);
        }

        public bool Delete(int id)
        {
            return _res.Delete(id);
        }
    }
}
