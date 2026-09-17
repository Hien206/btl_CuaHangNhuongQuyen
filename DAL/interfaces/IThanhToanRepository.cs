using System.Collections.Generic;
using Model;

namespace DAL.Interfaces
{
    public interface IThanhToanRepository
    {
        List<ThanhToanModel> GetAll();
        ThanhToanModel GetById(int id);
        bool Create(ThanhToanModel model);
    }
}
