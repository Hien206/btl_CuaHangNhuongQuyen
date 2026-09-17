using System.Collections.Generic;
using Model;

namespace BLL.Interfaces
{
    public interface IThongBaoBLL
    {
        List<ThongBaoModel> GetAll();
        ThongBaoModel GetById(int id);
        bool Create(ThongBaoModel model);
    }
}
