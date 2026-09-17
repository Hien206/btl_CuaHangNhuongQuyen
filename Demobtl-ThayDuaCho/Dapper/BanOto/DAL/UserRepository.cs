using DAL.Helper;
using Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL
{
    public partial class UserRepository : IUserRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        private sealed class UserSearchRow : UserModel
        {
            public long RecordCount { get; set; }
        }

        public UserRepository(IDatabaseHelper dbHelper) => _dbHelper = dbHelper;
        public bool Create(UserModel model) => Save("sp_user_create", model);
        public bool Update(UserModel model) => Save("sp_user_update", model);

        private bool Save(string procedureName, UserModel model)
        {
            _dbHelper.Execute(procedureName, new
            {
                model.user_id,
                model.hoten,
                model.ngaysinh,
                model.diachi,
                model.gioitinh,
                model.email,
                model.taikhoan,
                model.matkhau,
                model.role,
                model.image_url
            }, CommandType.StoredProcedure);
            return true;
        }

        public bool Delete(string id)
        {
            _dbHelper.Execute("sp_user_delete", new { user_id = id },
                CommandType.StoredProcedure);
            return true;
        }

        public UserModel GetUser(string username, string password) =>
            _dbHelper.QueryFirstOrDefault<UserModel>(
                "sp_user_get_by_username_password",
                new { taikhoan = username, matkhau = password },
                CommandType.StoredProcedure);

        public UserModel GetDatabyID(string id) =>
            _dbHelper.QueryFirstOrDefault<UserModel>("sp_user_get_by_id",
                new { user_id = id }, CommandType.StoredProcedure);

        public List<UserModel> Search(int pageIndex, int pageSize, out long total,
            string hoten, string taikhoan)
        {
            var rows = _dbHelper.Query<UserSearchRow>("sp_user_search", new
            {
                page_index = pageIndex,
                page_size = pageSize,
                hoten,
                taikhoan
            }, CommandType.StoredProcedure).ToList();
            total = rows.FirstOrDefault()?.RecordCount ?? 0;
            return rows.Cast<UserModel>().ToList();
        }
    }
}
