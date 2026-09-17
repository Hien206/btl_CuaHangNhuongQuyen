using System.Collections.Generic;
using BLL.Interfaces;
using DAL.Interfaces;
using Model;

namespace BLL
{
    public class ThongBaoBLL : IThongBaoBLL
    {
        private readonly IThongBaoRepository _res;

        public ThongBaoBLL(IThongBaoRepository res)
        {
            _res = res;
        }

        public List<ThongBaoModel> GetAll()
        {
            return _res.GetAll();
        }

        public ThongBaoModel GetById(int id)
        {
            return _res.GetById(id);
        }

        public bool Create(ThongBaoModel model)
        {
            return _res.Create(model);
        }
    }
}
