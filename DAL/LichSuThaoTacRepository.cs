using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DAL.Helper;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class LichSuThaoTacRepository : ILichSuThaoTacRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public LichSuThaoTacRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<LichSuThaoTacModel> GetAll()
        {
            string sql = "SELECT * FROM lichsuthaotac ORDER BY id DESC";
            DataTable dt = _dbHelper.ExecuteQuery(sql);
            return CollectionHelper.ConvertToList<LichSuThaoTacModel>(dt);
        }

        public bool Create(LichSuThaoTacModel model)
        {
            string sql = @"INSERT INTO lichsuthaotac (nguoidungid, hanhdong, tenbang, idbanghi, dulieucu, dulieumoi)
                           VALUES (@nguoidungid, @hanhdong, @tenbang, @idbanghi, @dulieucu, @dulieumoi)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@nguoidungid", model.nguoidungid ?? (object)DBNull.Value),
                new SqlParameter("@hanhdong", model.hanhdong),
                new SqlParameter("@tenbang", model.tenbang),
                new SqlParameter("@idbanghi", model.idbanghi ?? (object)DBNull.Value),
                new SqlParameter("@dulieucu", model.dulieucu ?? (object)DBNull.Value),
                new SqlParameter("@dulieumoi", model.dulieumoi ?? (object)DBNull.Value)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}
