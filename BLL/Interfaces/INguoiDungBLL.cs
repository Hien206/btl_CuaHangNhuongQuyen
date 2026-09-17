using System.Collections.Generic;
using Model;

namespace BLL.Interfaces
{
    public interface INguoiDungBLL
    {
        List<NguoiDungModel> GetAll();
        NguoiDungModel GetById(int id);
        NguoiDungModel Login(string email, string matkhau);
        bool Register(string email, string matkhau, string hoten, int vaitroid);
        bool Create(NguoiDungModel model);
        bool Update(NguoiDungModel model);
        bool Delete(int id);
    }
}
