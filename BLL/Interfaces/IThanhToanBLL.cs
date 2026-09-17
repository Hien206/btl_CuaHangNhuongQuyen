using System.Collections.Generic;
using Model;

namespace BLL.Interfaces
{
    public interface IThanhToanBLL
    {
        List<ThanhToanModel> GetAll();
        ThanhToanModel GetById(int id);
        bool Create(ThanhToanModel model);
    }
}
