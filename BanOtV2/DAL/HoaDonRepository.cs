using DAL.Helper;
using Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL
{
    public partial class HoaDonRepository : IHoaDonRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        private sealed class HoaDonRow
        {
            public string ma_hoa_don { get; set; }
            public string ho_ten { get; set; }
            public string dia_chi { get; set; }
            public string listjson_chitiet { get; set; }
            public long RecordCount { get; set; }
        }

        public HoaDonRepository(IDatabaseHelper dbHelper) => _dbHelper = dbHelper;
        public bool Create(HoaDonModel model) => Save("sp_hoa_don_create", model);
        public bool Update(HoaDonModel model) => Save("sp_hoa_don_update", model);

        private bool Save(string procedureName, HoaDonModel model)
        {
            _dbHelper.Execute(procedureName, new
            {
                model.ma_hoa_don,
                model.ho_ten,
                model.dia_chi,
                listjson_chitiet = model.listjson_chitiet == null
                    ? null
                    : MessageConvert.SerializeObject(model.listjson_chitiet)
            }, CommandType.StoredProcedure);
            return true;
        }

        public HoaDonModel GetDatabyID(string id)
        {
            var row = _dbHelper.QueryFirstOrDefault<HoaDonRow>("sp_hoa_don_get_by_id",
                new { ma_hoa_don = id }, CommandType.StoredProcedure);
            return ToModel(row);
        }

        public bool Delete(string id)
        {
            _dbHelper.Execute("sp_hoa_don_delete", new { ma_hoa_don = id },
                CommandType.StoredProcedure);
            return true;
        }

        public List<HoaDonModel> Search(int pageIndex, int pageSize, out long total,
            string hoten, string diachi)
        {
            var rows = _dbHelper.Query<HoaDonRow>("sp_hoa_don_search", new
            {
                page_index = pageIndex,
                page_size = pageSize,
                hoten,
                diachi
            }, CommandType.StoredProcedure).ToList();
            total = rows.FirstOrDefault()?.RecordCount ?? 0;
            return rows.Select(ToModel).ToList();
        }

        private static HoaDonModel ToModel(HoaDonRow row)
        {
            if (row == null) return null;
            return new HoaDonModel
            {
                ma_hoa_don = row.ma_hoa_don,
                ho_ten = row.ho_ten,
                dia_chi = row.dia_chi,
                listjson_chitiet = string.IsNullOrWhiteSpace(row.listjson_chitiet)
                    ? null
                    : row.listjson_chitiet.Replace("$", "")
                        .DeserializeObject<List<ChiTietHoaDonModel>>()
            };
        }
    }
}
