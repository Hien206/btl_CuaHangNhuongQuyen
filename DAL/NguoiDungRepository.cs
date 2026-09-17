using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DAL.Helper;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class NguoiDungRepository : INguoiDungRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public NguoiDungRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<NguoiDungModel> GetAll()
        {
            string sql = "SELECT * FROM nguoidung WHERE ngay_xoa IS NULL ORDER BY id ASC";
            DataTable dt = _dbHelper.ExecuteQuery(sql);
            return CollectionHelper.ConvertToList<NguoiDungModel>(dt);
        }

        public NguoiDungModel GetById(int id)
        {
            string sql = "SELECT * FROM nguoidung WHERE id = @id AND ngay_xoa IS NULL";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            DataTable dt = _dbHelper.ExecuteQuery(sql, parameters);
            var list = CollectionHelper.ConvertToList<NguoiDungModel>(dt);
            return list.Count > 0 ? list[0] : null;
        }

        public NguoiDungModel GetByEmail(string email)
        {
            string sql = "SELECT * FROM nguoidung WHERE email = @email AND ngay_xoa IS NULL";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@email", email)
            };
            DataTable dt = _dbHelper.ExecuteQuery(sql, parameters);
            var list = CollectionHelper.ConvertToList<NguoiDungModel>(dt);
            return list.Count > 0 ? list[0] : null;
        }

        public bool Create(NguoiDungModel model)
        {
            string sql = @"INSERT INTO nguoidung (email, matkhauhash, hoten, vaitroid, trang_thai_kich_hoat) 
                           VALUES (@email, @matkhauhash, @hoten, @vaitroid, @trang_thai_kich_hoat)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@email", model.email),
                new SqlParameter("@matkhauhash", model.matkhauhash),
                new SqlParameter("@hoten", model.hoten),
                new SqlParameter("@vaitroid", model.vaitroid),
                new SqlParameter("@trang_thai_kich_hoat", model.trang_thai_kich_hoat)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Update(NguoiDungModel model)
        {
            string sql = @"UPDATE nguoidung 
                           SET email = @email, hoten = @hoten, vaitroid = @vaitroid, ngay_cap_nhat = GETDATE() 
                           WHERE id = @id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", model.id),
                new SqlParameter("@email", model.email),
                new SqlParameter("@hoten", model.hoten),
                new SqlParameter("@vaitroid", model.vaitroid)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Delete(int id)
        {
            string sql = "UPDATE nguoidung SET ngay_xoa = GETDATE() WHERE id = @id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}
