using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Mvc
{
    public class EditRecord<T>
    {
        public EditRecord()
        {
            Result = true;
            Msg = "成功";
        }
        public bool Result { get; set; }
        public string Msg { get; set; }
        public T Dto { get; set; }
    }
}
