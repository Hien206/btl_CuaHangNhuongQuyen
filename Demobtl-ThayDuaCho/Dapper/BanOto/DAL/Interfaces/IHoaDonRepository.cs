using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public partial interface IHoaDonRepository
    {
        bool Create(HoaDonModel model);
        HoaDonModel GetDatabyID(string id);
        bool Update(HoaDonModel model);
        bool Delete(string id);
        List<HoaDonModel> Search(int pageIndex, int pageSize, out long total, string hoten, string diachi);
    }
}
