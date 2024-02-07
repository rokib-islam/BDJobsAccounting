using System.Data;
using System.Reflection;

namespace AccountingSystem.Web.HelperMethod
{
    public static class Helpers
    {
        public static DataTable ListiToDataTable<T>(List<T> items)
        {

            DataTable dt = new DataTable(typeof(T).Name);
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                dt.Columns.Add(property.Name);
            }
            foreach (T item in items)
            {
                var values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item, null);

                }
                dt.Rows.Add(values);
            }
            return dt;
        }
    }
}
