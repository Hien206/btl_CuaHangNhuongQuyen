using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public partial interface INewsBusiness
    {
        bool Create(NewsModel model);
    }
}
