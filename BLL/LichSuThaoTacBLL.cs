using System.Collections.Generic;
using BLL.Interfaces;
using DAL.Interfaces;
using Model;

namespace BLL
{
    public class LichSuThaoTacBLL : ILichSuThaoTacBLL
    {
        private readonly ILichSuThaoTacRepository _res;

        public LichSuThaoTacBLL(ILichSuThaoTacRepository res)
        {
            _res = res;
        }

        public List<LichSuThaoTacModel> GetAll()
        {
            return _res.GetAll();
        }

        public bool Create(LichSuThaoTacModel model)
        {
            return _res.Create(model);
        }
    }
}
