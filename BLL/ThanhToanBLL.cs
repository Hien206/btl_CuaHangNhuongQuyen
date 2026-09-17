using System.Collections.Generic;
using BLL.Interfaces;
using DAL.Interfaces;
using Model;

namespace BLL
{
    public class ThanhToanBLL : IThanhToanBLL
    {
        private readonly IThanhToanRepository _res;

        public ThanhToanBLL(IThanhToanRepository res)
        {
            _res = res;
        }

        public List<ThanhToanModel> GetAll()
        {
            return _res.GetAll();
        }

        public ThanhToanModel GetById(int id)
        {
            return _res.GetById(id);
        }

        public bool Create(ThanhToanModel model)
        {
            return _res.Create(model);
        }
    }
}
