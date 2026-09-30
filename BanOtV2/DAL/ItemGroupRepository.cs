using DAL.Helper;
using Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL
{
    public partial class ItemGroupRepository : IItemGroupRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        public ItemGroupRepository(IDatabaseHelper dbHelper) => _dbHelper = dbHelper;
        public List<ItemGroupModel> GetData() =>
            _dbHelper.Query<ItemGroupModel>("sp_item_group_get_data",
                commandType: CommandType.StoredProcedure).ToList();
    }
}
