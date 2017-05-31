using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mvc.Validate
{
    /// <summary>
    ///  提供一个通用特性，指定实体是否作为一个查询参数
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class QueryParamAttribute : Attribute
    {
        public QueryParamAttribute()
        {
            AllowQuery = true;
        }
        public QueryParamAttribute(bool allowQuery)
        {
            AllowQuery = allowQuery;
        }
        public bool AllowQuery { get; set; }
    }
}
