using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DAL.Helper;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class VaiTroRepository : IVaiTroRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public VaiTroRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<VaiTroModel> GetAll()
        {
            string sql = "SELECT * FROM vaitro ORDER BY id ASC";
            DataTable dt = _dbHelper.ExecuteQuery(sql);
            return CollectionHelper.ConvertToList<VaiTroModel>(dt);
        }

        public VaiTroModel GetById(int id)
        {
            string sql = "SELECT * FROM vaitro WHERE id = @id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            DataTable dt = _dbHelper.ExecuteQuery(sql, parameters);
            var list = CollectionHelper.ConvertToList<VaiTroModel>(dt);
            return list.Count > 0 ? list[0] : null;
        }

        public bool Create(VaiTroModel model)
        {
            string sql = "INSERT INTO vaitro (mavaitro, tenvaitro) VALUES (@mavaitro, @tenvaitro)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@mavaitro", model.mavaitro),
                new SqlParameter("@tenvaitro", model.tenvaitro)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Update(VaiTroModel model)
        {
            string sql = "UPDATE vaitro SET mavaitro = @mavaitro, tenvaitro = @tenvaitro WHERE id = @id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", model.id),
                new SqlParameter("@mavaitro", model.mavaitro),
                new SqlParameter("@tenvaitro", model.tenvaitro)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Delete(int id)
        {
            string sql = "DELETE FROM vaitro WHERE id = @id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}
