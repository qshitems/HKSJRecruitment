using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mvc.Validate
{
    /// <summary>
    /// 指示数据字段是否可在详情页面上显示
    /// 设置此特性后，列表页面也适用
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DetailableAttribute : Attribute
    {
        public DetailableAttribute()
        {
            AllowDetail = false;
        }
        public DetailableAttribute(bool allowDetail)
        {
            AllowDetail = allowDetail;
        }
        public bool AllowDetail { get; set; }
    }
}
