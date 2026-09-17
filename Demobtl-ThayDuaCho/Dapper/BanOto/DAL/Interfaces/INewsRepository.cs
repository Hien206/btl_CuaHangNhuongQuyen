using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public interface INewsRepository
    {
        bool Create(NewsModel model);
    }
}
