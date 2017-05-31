using System;
using System.Collections.Generic;

namespace HKSJRecruitment.Models.Models
{
    public partial class Tapp_Param
    {
        public Tapp_Param()
        {
            this.THR_Recruit = new List<THR_Recruit>();
        }

        public int Id { get; set; }
        public Nullable<int> Parentid { get; set; }
        public string ParamsName { get; set; }
        public Nullable<int> Sort { get; set; }
        public Nullable<int> IsDelete { get; set; }
        public string Remark { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditTime { get; set; }
        public string DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteTime { get; set; }
        public virtual ICollection<THR_Recruit> THR_Recruit { get; set; }
    }
}
