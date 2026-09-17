using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DAL.Helper;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class HoaDonRepository : IHoaDonRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public HoaDonRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<HoaDonModel> GetAll()
        {
            string sql = "SELECT * FROM hoadon WHERE ngayxoa IS NULL ORDER BY id ASC";
            DataTable dt = _dbHelper.ExecuteQuery(sql);
            return CollectionHelper.ConvertToList<HoaDonModel>(dt);
        }

        public HoaDonModel GetById(int id)
        {
            string sql = "SELECT * FROM hoadon WHERE id = @id AND ngayxoa IS NULL";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            DataTable dt = _dbHelper.ExecuteQuery(sql, parameters);
            var list = CollectionHelper.ConvertToList<HoaDonModel>(dt);
            return list.Count > 0 ? list[0] : null;
        }

        public bool Create(HoaDonModel model)
        {
            string sql = @"INSERT INTO hoadon (cuahangid, hopdongid, kythanhtoan, tienphinhuongquyen, tienthue, tongtien, trangthai, duongdanpdf)
                           VALUES (@cuahangid, @hopdongid, @kythanhtoan, @tienphinhuongquyen, @tienthue, @tongtien, @trangthai, @duongdanpdf)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@cuahangid", model.cuahangid),
                new SqlParameter("@hopdongid", model.hopdongid),
                new SqlParameter("@kythanhtoan", model.kythanhtoan),
                new SqlParameter("@tienphinhuongquyen", model.tienphinhuongquyen),
                new SqlParameter("@tienthue", model.tienthue),
                new SqlParameter("@tongtien", model.tongtien),
                new SqlParameter("@trangthai", string.IsNullOrEmpty(model.trangthai) ? "chuathanhtoan" : model.trangthai),
                new SqlParameter("@duongdanpdf", model.duongdanpdf ?? (object)DBNull.Value)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Update(HoaDonModel model)
        {
            string sql = @"UPDATE hoadon
                           SET cuahangid = @cuahangid, hopdongid = @hopdongid, kythanhtoan = @kythanhtoan,
                               tienphinhuongquyen = @tienphinhuongquyen, tienthue = @tienthue, tongtien = @tongtien,
                               trangthai = @trangthai, duongdanpdf = @duongdanpdf
                           WHERE id = @id AND ngayxoa IS NULL";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", model.id),
                new SqlParameter("@cuahangid", model.cuahangid),
                new SqlParameter("@hopdongid", model.hopdongid),
                new SqlParameter("@kythanhtoan", model.kythanhtoan),
                new SqlParameter("@tienphinhuongquyen", model.tienphinhuongquyen),
                new SqlParameter("@tienthue", model.tienthue),
                new SqlParameter("@tongtien", model.tongtien),
                new SqlParameter("@trangthai", model.trangthai),
                new SqlParameter("@duongdanpdf", model.duongdanpdf ?? (object)DBNull.Value)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Delete(int id)
        {
            string sql = "UPDATE hoadon SET ngayxoa = GETDATE() WHERE id = @id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}
