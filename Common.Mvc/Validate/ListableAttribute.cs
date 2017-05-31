using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mvc.Validate
{
    /// <summary>
    /// 指示数据字段是否可在列表页面上显示
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ListableAttribute : Attribute
    {
        public ListableAttribute()
        {
            AllowList = false;
        }
        public ListableAttribute(bool allowList)
        {
            AllowList = allowList;
        }
        public bool AllowList { get; set; }
    }
}
