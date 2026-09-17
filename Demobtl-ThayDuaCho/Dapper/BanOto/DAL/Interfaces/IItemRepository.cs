using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public partial interface IItemRepository
    {
        bool Create(ItemModel model);
        bool Update(ItemModel model);
        ItemModel GetDatabyID(string id);
        List<ItemModel> GetDataAll();
        List<ItemModel> Search(int pageIndex, int pageSize, out long total, string item_group_id, string item_name);
    }
}
