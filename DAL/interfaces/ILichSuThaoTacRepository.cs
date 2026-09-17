using System.Collections.Generic;
using Model;

namespace DAL.Interfaces
{
    public interface ILichSuThaoTacRepository
    {
        List<LichSuThaoTacModel> GetAll();
        bool Create(LichSuThaoTacModel model);
    }
}
