using DAL.Helper;
using Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL
{
    public partial class ItemRepository : IItemRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        private sealed class ItemSearchRow : ItemModel
        {
            public long RecordCount { get; set; }
        }

        public ItemRepository(IDatabaseHelper dbHelper) => _dbHelper = dbHelper;
        public bool Create(ItemModel model) => Save("sp_item_create", model);
        public bool Update(ItemModel model) => Save("sp_item_update", model);

        private bool Save(string procedureName, ItemModel model)
        {
            _dbHelper.Execute(procedureName, new
            {
                model.item_id,
                model.item_group_id,
                model.item_image,
                model.item_name,
                model.item_price
            }, CommandType.StoredProcedure);
            return true;
        }

        public ItemModel GetDatabyID(string id) =>
            _dbHelper.QueryFirstOrDefault<ItemModel>("sp_item_get_by_id",
                new { item_id = id }, CommandType.StoredProcedure);

        public List<ItemModel> GetDataAll() =>
            _dbHelper.Query<ItemModel>("sp_item_all",
                commandType: CommandType.StoredProcedure).ToList();

        public List<ItemModel> Search(int pageIndex, int pageSize, out long total,
            string item_group_id, string item_name)
        {
            var rows = _dbHelper.Query<ItemSearchRow>("sp_item_search", new
            {
                page_index = pageIndex,
                page_size = pageSize,
                item_name,
                item_group_id
            }, CommandType.StoredProcedure).ToList();
            total = rows.FirstOrDefault()?.RecordCount ?? 0;
            return rows.Cast<ItemModel>().ToList();
        }
    }
}
