using System.Collections.Generic;
using Model;

namespace DAL.Interfaces
{
    public interface INguoiDungRepository
    {
        List<NguoiDungModel> GetAll();
        NguoiDungModel GetById(int id);
        NguoiDungModel GetByEmail(string email);
        bool Create(NguoiDungModel model);
        bool Update(NguoiDungModel model);
        bool Delete(int id);
    }
}
