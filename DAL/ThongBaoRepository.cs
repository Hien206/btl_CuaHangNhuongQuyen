using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DAL.Helper;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class ThongBaoRepository : IThongBaoRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public ThongBaoRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<ThongBaoModel> GetAll()
        {
            string sql = "SELECT * FROM thongbao ORDER BY id DESC";
            DataTable dt = _dbHelper.ExecuteQuery(sql);
            return CollectionHelper.ConvertToList<ThongBaoModel>(dt);
        }

        public ThongBaoModel GetById(int id)
        {
            string sql = "SELECT * FROM thongbao WHERE id = @id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            DataTable dt = _dbHelper.ExecuteQuery(sql, parameters);
            var list = CollectionHelper.ConvertToList<ThongBaoModel>(dt);
            return list.Count > 0 ? list[0] : null;
        }

        public bool Create(ThongBaoModel model)
        {
            string sql = @"INSERT INTO thongbao (nguoidungid, loaithongbao, noidung, dadoc)
                           VALUES (@nguoidungid, @loaithongbao, @noidung, @dadoc)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@nguoidungid", model.nguoidungid),
                new SqlParameter("@loaithongbao", model.loaithongbao),
                new SqlParameter("@noidung", model.noidung),
                new SqlParameter("@dadoc", model.dadoc)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Delete(int id)
        {
            string sql = "DELETE FROM thongbao WHERE id = @id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool DeleteAll()
        {
            string sql = "DELETE FROM thongbao";
            return _dbHelper.ExecuteNonQuery(sql) >= 0;
        }

        public int CleanupDuplicates()
        {
            // Dọn dẹp các thông báo trùng lặp (giữ lại 1 bản ghi mới nhất cho mỗi nội dung trong ngày)
            string sql = @"WITH CTE AS (
                               SELECT id, ROW_NUMBER() OVER (
                                   PARTITION BY loaithongbao, noidung, CAST(ngaytao AS DATE)
                                   ORDER BY id DESC
                               ) AS rn
                               FROM thongbao
                           )
                           DELETE FROM CTE WHERE rn > 1;";
            return _dbHelper.ExecuteNonQuery(sql);
        }
    }
}
