using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DAL.Helper;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class ThanhToanRepository : IThanhToanRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public ThanhToanRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<ThanhToanModel> GetAll()
        {
            string sql = "SELECT * FROM thanhtoan ORDER BY id ASC";
            DataTable dt = _dbHelper.ExecuteQuery(sql);
            return CollectionHelper.ConvertToList<ThanhToanModel>(dt);
        }

        public ThanhToanModel GetById(int id)
        {
            string sql = "SELECT * FROM thanhtoan WHERE id = @id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            DataTable dt = _dbHelper.ExecuteQuery(sql, parameters);
            var list = CollectionHelper.ConvertToList<ThanhToanModel>(dt);
            return list.Count > 0 ? list[0] : null;
        }

        public bool Create(ThanhToanModel model)
        {
            string sql = @"INSERT INTO thanhtoan (hoadonid, sotien, phuongthuc, magiaodich, ngaythanhtoan)
                           VALUES (@hoadonid, @sotien, @phuongthuc, @magiaodich, GETDATE())";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@hoadonid", model.hoadonid),
                new SqlParameter("@sotien", model.sotien),
                new SqlParameter("@phuongthuc", string.IsNullOrEmpty(model.phuongthuc) ? "chuyenkhoan" : model.phuongthuc),
                new SqlParameter("@magiaodich", model.magiaodich ?? (object)DBNull.Value)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}
