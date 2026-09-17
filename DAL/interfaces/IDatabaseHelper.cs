using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL.Interfaces
{
    public interface IDatabaseHelper
    {
        string SetConnectionString(string connectionString);
        string GetConnectionString();

        DataTable ExecuteSprocTable(string sprocName, params object[] parameters);
        object ExecuteScalarSproc(string sprocName, params object[] parameters);
        string ExecuteSprocText(string sprocName, params object[] parameters);

        DataTable ExecuteQuery(string sql, params SqlParameter[] parameters);
        int ExecuteNonQuery(string sql, params SqlParameter[] parameters);
        object ExecuteScalar(string sql, params SqlParameter[] parameters);
    }
}
