using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DAL.Helper;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class CuaHangRepository : ICuaHangRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public CuaHangRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<CuaHangModel> GetAll()
        {
            string sql = "SELECT * FROM cuahang WHERE ngay_xoa IS NULL ORDER BY id ASC";
            DataTable dt = _dbHelper.ExecuteQuery(sql);
            return CollectionHelper.ConvertToList<CuaHangModel>(dt);
        }

        public CuaHangModel GetById(int id)
        {
            string sql = "SELECT * FROM cuahang WHERE id = @id AND ngay_xoa IS NULL";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            DataTable dt = _dbHelper.ExecuteQuery(sql, parameters);
            var list = CollectionHelper.ConvertToList<CuaHangModel>(dt);
            return list.Count > 0 ? list[0] : null;
        }

        public bool Create(CuaHangModel model)
        {
            string sql = @"INSERT INTO cuahang (macuahang, tencuahang, chu_cua_hang_id, diachi, trangthai, nguoi_tao_id)
                           VALUES (@macuahang, @tencuahang, @chu_cua_hang_id, @diachi, @trangthai, @nguoi_tao_id)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@macuahang", model.macuahang),
                new SqlParameter("@tencuahang", model.tencuahang),
                new SqlParameter("@chu_cua_hang_id", model.chu_cua_hang_id > 0 ? model.chu_cua_hang_id : 1),
                new SqlParameter("@diachi", model.diachi ?? (object)DBNull.Value),
                new SqlParameter("@trangthai", string.IsNullOrEmpty(model.trangthai) ? "hoatdong" : model.trangthai),
                new SqlParameter("@nguoi_tao_id", (model.nguoi_tao_id.HasValue && model.nguoi_tao_id.Value > 0) ? model.nguoi_tao_id.Value : (object)DBNull.Value)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Update(CuaHangModel model)
        {
            string sql = @"UPDATE cuahang
                           SET macuahang = @macuahang, tencuahang = @tencuahang, chu_cua_hang_id = @chu_cua_hang_id,
                               diachi = @diachi, trangthai = @trangthai, ngay_cap_nhat = GETDATE()
                           WHERE id = @id AND ngay_xoa IS NULL";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", model.id),
                new SqlParameter("@macuahang", model.macuahang),
                new SqlParameter("@tencuahang", model.tencuahang),
                new SqlParameter("@chu_cua_hang_id", model.chu_cua_hang_id),
                new SqlParameter("@diachi", model.diachi ?? (object)DBNull.Value),
                new SqlParameter("@trangthai", model.trangthai)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Delete(int id)
        {
            string sql = "UPDATE cuahang SET ngay_xoa = GETDATE() WHERE id = @id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}
