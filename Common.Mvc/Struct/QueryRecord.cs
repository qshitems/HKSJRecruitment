using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Mvc
{
    public class QueryRecord<T>
    {
        public QueryRecord()
        {
            Result = true; 
            Msg = "成功";
        }
        public bool Result { get; set; }
        public string Msg { get; set; }
        public int Total;
        public List<T> Rows;
    }
}
