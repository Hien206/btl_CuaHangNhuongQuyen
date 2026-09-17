using System.Collections.Generic;
using Model;

namespace BLL.Interfaces
{
    public interface ILichSuThaoTacBLL
    {
        List<LichSuThaoTacModel> GetAll();
        bool Create(LichSuThaoTacModel model);
    }
}
