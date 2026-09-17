using System.Collections.Generic;
using BLL.Interfaces;
using DAL.Interfaces;
using Model;

namespace BLL
{
    public class KhuyenMaiBLL : IKhuyenMaiBLL
    {
        private readonly IKhuyenMaiRepository _res;

        public KhuyenMaiBLL(IKhuyenMaiRepository res)
        {
            _res = res;
        }

        public List<KhuyenMaiModel> GetAll()
        {
            return _res.GetAll();
        }

        public KhuyenMaiModel GetById(int id)
        {
            return _res.GetById(id);
        }

        public bool Create(KhuyenMaiModel model)
        {
            return _res.Create(model);
        }

        public bool Update(KhuyenMaiModel model)
        {
            return _res.Update(model);
        }

        public bool Delete(int id)
        {
            return _res.Delete(id);
        }
    }
}
