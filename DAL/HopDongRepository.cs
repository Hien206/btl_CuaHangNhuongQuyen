using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DAL.Helper;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class HopDongRepository : IHopDongRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public HopDongRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<HopDongModel> GetAll()
        {
            string sql = "SELECT * FROM hopdong WHERE ngayxoa IS NULL ORDER BY id ASC";
            DataTable dt = _dbHelper.ExecuteQuery(sql);
            return CollectionHelper.ConvertToList<HopDongModel>(dt);
        }

        public HopDongModel GetById(int id)
        {
            string sql = "SELECT * FROM hopdong WHERE id = @id AND ngayxoa IS NULL";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            DataTable dt = _dbHelper.ExecuteQuery(sql, parameters);
            var list = CollectionHelper.ConvertToList<HopDongModel>(dt);
            return list.Count > 0 ? list[0] : null;
        }

        public bool Create(HopDongModel model)
        {
            string sql = @"INSERT INTO hopdong (cuahangid, sohopdong, phantramphi, ngaybatdau, ngayketthuc, trangthai, duongdanpdf, nguoitaoid)
                           VALUES (@cuahangid, @sohopdong, @phantramphi, @ngaybatdau, @ngayketthuc, @trangthai, @duongdanpdf, @nguoitaoid)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@cuahangid", model.cuahangid),
                new SqlParameter("@sohopdong", model.sohopdong),
                new SqlParameter("@phantramphi", model.phantramphi),
                new SqlParameter("@ngaybatdau", model.ngaybatdau),
                new SqlParameter("@ngayketthuc", model.ngayketthuc),
                new SqlParameter("@trangthai", string.IsNullOrEmpty(model.trangthai) ? "hieuluc" : model.trangthai),
                new SqlParameter("@duongdanpdf", model.duongdanpdf ?? (object)DBNull.Value),
                new SqlParameter("@nguoitaoid", (model.nguoitaoid.HasValue && model.nguoitaoid.Value > 0) ? model.nguoitaoid.Value : (object)DBNull.Value)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Update(HopDongModel model)
        {
            string sql = @"UPDATE hopdong
                           SET cuahangid = @cuahangid, sohopdong = @sohopdong, phantramphi = @phantramphi,
                               ngaybatdau = @ngaybatdau, ngayketthuc = @ngayketthuc, trangthai = @trangthai,
                               duongdanpdf = @duongdanpdf, ngaycapnhat = GETDATE()
                           WHERE id = @id AND ngayxoa IS NULL";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", model.id),
                new SqlParameter("@cuahangid", model.cuahangid),
                new SqlParameter("@sohopdong", model.sohopdong),
                new SqlParameter("@phantramphi", model.phantramphi),
                new SqlParameter("@ngaybatdau", model.ngaybatdau),
                new SqlParameter("@ngayketthuc", model.ngayketthuc),
                new SqlParameter("@trangthai", model.trangthai),
                new SqlParameter("@duongdanpdf", model.duongdanpdf ?? (object)DBNull.Value)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Delete(int id)
        {
            string sql = "UPDATE hopdong SET ngayxoa = GETDATE() WHERE id = @id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}
