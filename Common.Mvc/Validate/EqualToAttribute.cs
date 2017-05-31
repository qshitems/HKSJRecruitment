using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mvc.Validate
{
    /// <summary>
    /// 比较两个控件的值是否相等
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EqualToAttribute : Attribute
    {
        public EqualToAttribute(string ctrlId)
        {
            CtrlId = ctrlId;
        }
        public string CtrlId { get; set; }
    }
}
