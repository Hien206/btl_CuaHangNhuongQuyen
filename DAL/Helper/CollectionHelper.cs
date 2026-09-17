using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DAL.Helper
{
    public static class CollectionHelper
    {
        public static List<T> ConvertToList<T>(DataTable dt) where T : new()
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr) where T : new()
        {
            Type temp = typeof(T);
            T obj = new T();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name.Equals(column.ColumnName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (dr[column.ColumnName] != DBNull.Value)
                        {
                            Type t = Nullable.GetUnderlyingType(pro.PropertyType) ?? pro.PropertyType;
                            object safeValue = Convert.ChangeType(dr[column.ColumnName], t);
                            pro.SetValue(obj, safeValue, null);
                        }
                        break;
                    }
                }
            }
            return obj;
        }
    }
}
