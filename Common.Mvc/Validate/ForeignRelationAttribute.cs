using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mvc.Validate
{
    /// <summary>
    ///  提供一个通用特性，使您可以为属性指定外键对应的导航属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ForeignRelationAttribute : Attribute
    {
        public ForeignRelationAttribute(string navProperty)
        {
            NavProperty = navProperty;
        }
        public string NavProperty { get; set; }
    }
}
