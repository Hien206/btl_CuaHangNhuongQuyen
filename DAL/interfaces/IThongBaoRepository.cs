using System.Collections.Generic;
using Model;

namespace DAL.Interfaces
{
    public interface IThongBaoRepository
    {
        List<ThongBaoModel> GetAll();
        ThongBaoModel GetById(int id);
        bool Create(ThongBaoModel model);
    }
}
