using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DAL.Helper;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class KhuyenMaiRepository : IKhuyenMaiRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public KhuyenMaiRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<KhuyenMaiModel> GetAll()
        {
            string sql = "SELECT * FROM khuyenmai WHERE ngayxoa IS NULL ORDER BY id ASC";
            DataTable dt = _dbHelper.ExecuteQuery(sql);
            return CollectionHelper.ConvertToList<KhuyenMaiModel>(dt);
        }

        public KhuyenMaiModel GetById(int id)
        {
            string sql = "SELECT * FROM khuyenmai WHERE id = @id AND ngayxoa IS NULL";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            DataTable dt = _dbHelper.ExecuteQuery(sql, parameters);
            var list = CollectionHelper.ConvertToList<KhuyenMaiModel>(dt);
            return list.Count > 0 ? list[0] : null;
        }

        public bool Create(KhuyenMaiModel model)
        {
            string sql = @"INSERT INTO khuyenmai (cuahangid, tenkhuyenmai, phantramgiam, ngaybatdau, ngayketthuc, nguoitaoid)
                           VALUES (@cuahangid, @tenkhuyenmai, @phantramgiam, @ngaybatdau, @ngayketthuc, @nguoitaoid)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@cuahangid", (model.cuahangid.HasValue && model.cuahangid.Value > 0) ? model.cuahangid.Value : (object)DBNull.Value),
                new SqlParameter("@tenkhuyenmai", model.tenkhuyenmai),
                new SqlParameter("@phantramgiam", model.phantramgiam),
                new SqlParameter("@ngaybatdau", model.ngaybatdau),
                new SqlParameter("@ngayketthuc", model.ngayketthuc),
                new SqlParameter("@nguoitaoid", (model.nguoitaoid.HasValue && model.nguoitaoid.Value > 0) ? model.nguoitaoid.Value : (object)DBNull.Value)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Update(KhuyenMaiModel model)
        {
            string sql = @"UPDATE khuyenmai
                           SET cuahangid = @cuahangid, tenkhuyenmai = @tenkhuyenmai, phantramgiam = @phantramgiam,
                               ngaybatdau = @ngaybatdau, ngayketthuc = @ngayketthuc
                           WHERE id = @id AND ngayxoa IS NULL";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", model.id),
                new SqlParameter("@cuahangid", model.cuahangid ?? (object)DBNull.Value),
                new SqlParameter("@tenkhuyenmai", model.tenkhuyenmai),
                new SqlParameter("@phantramgiam", model.phantramgiam),
                new SqlParameter("@ngaybatdau", model.ngaybatdau),
                new SqlParameter("@ngayketthuc", model.ngayketthuc)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Delete(int id)
        {
            string sql = "UPDATE khuyenmai SET ngayxoa = GETDATE() WHERE id = @id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}
