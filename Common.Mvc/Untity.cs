using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.Mvc
{
    public static class Untity
    {
        public static T CopyObject<T>(T dest, T src)
        {
            string[] excepts = new string[] { "Id", "CreateBy", "CreateName", "CreateTime", "EditBy", "EditName", "EditTime", "IsDelete", "DeleteBy", "DeleteName", "DeleteTime" };
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                Type propertyType = property.PropertyType;
                string propertyTypeName = propertyType.Name.ToLower();
                if ((propertyType.IsValueType || propertyTypeName == "string")
                    && (excepts.Count(c => c.Equals(property.Name, StringComparison.OrdinalIgnoreCase)) == 0))
                {
                    object value = property.GetValue(src, null);
                    //if (value != null || (propertyTypeName == "string"))
                    //{
                    property.SetValue(dest, value, null);
                    //}
                }
            }
            return dest;
        }
    }
}
