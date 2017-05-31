using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mvc.Validate
{
    /// <summary>
    ///  提供一个通用特性，使您可以为实体类指定本地化的字符串,即实体类显示名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DisplayModelAttribute : Attribute
    {
        public DisplayModelAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}
