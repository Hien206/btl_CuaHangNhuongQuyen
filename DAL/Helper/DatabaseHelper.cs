using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DAL.Interfaces;

namespace DAL.Helper
{
    public class DatabaseHelper : IDatabaseHelper
    {
        private string _connectionString;

        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            return _connectionString;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }

        public DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        public DataTable ExecuteSprocTable(string sprocName, params object[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sprocName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlCommandBuilder.DeriveParameters(cmd);

                    if (parameters != null)
                    {
                        int paramIndex = 0;
                        for (int i = 0; i < cmd.Parameters.Count; i++)
                        {
                            if (cmd.Parameters[i].Direction == ParameterDirection.Input ||
                                cmd.Parameters[i].Direction == ParameterDirection.InputOutput)
                            {
                                if (paramIndex < parameters.Length)
                                {
                                    cmd.Parameters[i].Value = parameters[paramIndex] ?? DBNull.Value;
                                    paramIndex++;
                                }
                            }
                        }
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public object ExecuteScalarSproc(string sprocName, params object[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sprocName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlCommandBuilder.DeriveParameters(cmd);

                    if (parameters != null)
                    {
                        int paramIndex = 0;
                        for (int i = 0; i < cmd.Parameters.Count; i++)
                        {
                            if (cmd.Parameters[i].Direction == ParameterDirection.Input ||
                                cmd.Parameters[i].Direction == ParameterDirection.InputOutput)
                            {
                                if (paramIndex < parameters.Length)
                                {
                                    cmd.Parameters[i].Value = parameters[paramIndex] ?? DBNull.Value;
                                    paramIndex++;
                                }
                            }
                        }
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }

        public string ExecuteSprocText(string sprocName, params object[] parameters)
        {
            var res = ExecuteScalarSproc(sprocName, parameters);
            return res != null ? res.ToString() : string.Empty;
        }
    }
}
