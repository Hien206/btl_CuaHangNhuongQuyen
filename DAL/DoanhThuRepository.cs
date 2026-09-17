using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DAL.Helper;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class DoanhThuRepository : IDoanhThuRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public DoanhThuRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<DoanhThuModel> GetAll()
        {
            string sql = "SELECT * FROM doanhthu WHERE ngayxoa IS NULL ORDER BY id ASC";
            DataTable dt = _dbHelper.ExecuteQuery(sql);
            return CollectionHelper.ConvertToList<DoanhThuModel>(dt);
        }

        public DoanhThuModel GetById(int id)
        {
            string sql = "SELECT * FROM doanhthu WHERE id = @id AND ngayxoa IS NULL";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            DataTable dt = _dbHelper.ExecuteQuery(sql, parameters);
            var list = CollectionHelper.ConvertToList<DoanhThuModel>(dt);
            return list.Count > 0 ? list[0] : null;
        }

        public bool Create(DoanhThuModel model)
        {
            string sql = @"INSERT INTO doanhthu (cuahangid, ngaybaocao, tongdoanhthu, ghichu, trangthai, nguoinhapid)
                           VALUES (@cuahangid, @ngaybaocao, @tongdoanhthu, @ghichu, @trangthai, @nguoinhapid)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@cuahangid", model.cuahangid),
                new SqlParameter("@ngaybaocao", model.ngaybaocao),
                new SqlParameter("@tongdoanhthu", model.tongdoanhthu),
                new SqlParameter("@ghichu", model.ghichu ?? (object)DBNull.Value),
                new SqlParameter("@trangthai", string.IsNullOrEmpty(model.trangthai) ? "dunghan" : model.trangthai),
                new SqlParameter("@nguoinhapid", (model.nguoinhapid.HasValue && model.nguoinhapid.Value > 0) ? model.nguoinhapid.Value : (object)DBNull.Value)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Update(DoanhThuModel model)
        {
            string sql = @"UPDATE doanhthu
                           SET cuahangid = @cuahangid, ngaybaocao = @ngaybaocao, tongdoanhthu = @tongdoanhthu,
                               ghichu = @ghichu, trangthai = @trangthai
                           WHERE id = @id AND ngayxoa IS NULL";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", model.id),
                new SqlParameter("@cuahangid", model.cuahangid),
                new SqlParameter("@ngaybaocao", model.ngaybaocao),
                new SqlParameter("@tongdoanhthu", model.tongdoanhthu),
                new SqlParameter("@ghichu", model.ghichu ?? (object)DBNull.Value),
                new SqlParameter("@trangthai", model.trangthai)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Delete(int id)
        {
            string sql = "UPDATE doanhthu SET ngayxoa = GETDATE() WHERE id = @id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}
