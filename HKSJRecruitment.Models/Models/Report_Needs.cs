using System;
using System.Collections.Generic;

namespace HKSJRecruitment.Models.Models
{
    public partial class Report_Needs
    {
        public int id { get; set; }
        public string deptname { get; set; }
        public string PositionName { get; set; }
        public Nullable<int> cnt { get; set; }
        public Nullable<System.DateTime> createtime { get; set; }
    }
}
