using System;
using System.Collections.Generic;

namespace HKSJRecruitment.Models.Models
{
    public partial class TErp_Post
    {
        public int Id { get; set; }
        public string PostName { get; set; }
        public string PostDesc { get; set; }
        public int PositionId { get; set; }
        public int DeptId { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditTime { get; set; }
        public int IsDelete { get; set; }
        public string DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteTime { get; set; }
        public virtual TErp_Department TErp_Department { get; set; }
        public virtual TErp_Position TErp_Position { get; set; }
    }
}
