using DAL.Helper;
using Model;
using System.Data;

namespace DAL
{
    public partial class NewsRepository : INewsRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        public NewsRepository(IDatabaseHelper dbHelper) => _dbHelper = dbHelper;
        public bool Create(NewsModel model)
        {
            _dbHelper.Execute("sp_news_create", new
            {
                model.news_id,
                model.title,
                model.content_news
            }, CommandType.StoredProcedure);
            return true;
        }
    }
}
