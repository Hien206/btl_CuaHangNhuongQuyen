using System.Collections.Generic;
using System.Data;

namespace DAL.Helper
{
    public interface IDatabaseHelper
    {
        int Execute(string commandText, object parameters = null,
            CommandType commandType = CommandType.Text);
        T ExecuteScalar<T>(string commandText, object parameters = null,
            CommandType commandType = CommandType.Text);
        IEnumerable<T> Query<T>(string commandText, object parameters = null,
            CommandType commandType = CommandType.Text);
        T QueryFirstOrDefault<T>(string commandText, object parameters = null,
            CommandType commandType = CommandType.Text);
    }
}
