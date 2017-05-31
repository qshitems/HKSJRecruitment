using System;
using System.Collections.Generic;

namespace HKSJRecruitment.Models.Models
{
    public partial class THR_Recruit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public Nullable<int> DptId { get; set; }
        public Nullable<int> PostId { get; set; }
        public Nullable<int> Status { get; set; }
        public string Interviewer { get; set; }
        public Nullable<System.DateTime> Interview { get; set; }
        public string Remark { get; set; }
        public string Userurl { get; set; }
        public Nullable<System.DateTime> HireTime { get; set; }
        public string Email { get; set; }
        public Nullable<int> NeedsId { get; set; }
        public Nullable<int> EntryType { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditTime { get; set; }
        public Nullable<int> IsDelete { get; set; }
        public string DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteTime { get; set; }
        public virtual Tapp_Param Tapp_Param { get; set; }
        public virtual TErp_Department TErp_Department { get; set; }
        public virtual TErp_Position TErp_Position { get; set; }
        public virtual THR_Needs THR_Needs { get; set; }
    }
}
