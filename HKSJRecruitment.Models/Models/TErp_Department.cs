using System;
using System.Collections.Generic;

namespace HKSJRecruitment.Models.Models
{
    public partial class TErp_Department
    {
        public TErp_Department()
        {
            this.TErp_Post = new List<TErp_Post>();
            this.THR_Needs = new List<THR_Needs>();
            this.THR_Recruit = new List<THR_Recruit>();
        }

        public int Id { get; set; }
        public string DeptName { get; set; }
        public string DeptType { get; set; }
        public string DeptIcon { get; set; }
        public string DeptMaster { get; set; }
        public string DeptTel { get; set; }
        public string DeptFax { get; set; }
        public string DeptAddr { get; set; }
        public string DeptEmail { get; set; }
        public string DeptDesc { get; set; }
        public int SeqNo { get; set; }
        public int ParentId { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditTime { get; set; }
        public int IsDelete { get; set; }
        public string DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteTime { get; set; }
        public virtual ICollection<TErp_Post> TErp_Post { get; set; }
        public virtual ICollection<THR_Needs> THR_Needs { get; set; }
        public virtual ICollection<THR_Recruit> THR_Recruit { get; set; }
    }
}
