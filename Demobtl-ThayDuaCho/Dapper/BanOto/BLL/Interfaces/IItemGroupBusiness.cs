using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public partial interface IItemGroupBusiness
    {
        List<ItemGroupModel> GetData();
    }
}
