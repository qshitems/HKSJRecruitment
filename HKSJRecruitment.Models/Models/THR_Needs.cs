using System;
using System.Collections.Generic;

namespace HKSJRecruitment.Models.Models
{
    public partial class THR_Needs
    {
        public THR_Needs()
        {
            this.THR_Recruit = new List<THR_Recruit>();
        }

        public int Id { get; set; }
        public int DeptId { get; set; }
        public int PostId { get; set; }
        public int NeedQuantity { get; set; }
        public int HaveBeenQuantity { get; set; }
        public string Demander { get; set; }
        public Nullable<System.DateTime> CutTime { get; set; }
        public string Remarks { get; set; }
        public int IsDelete { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditTime { get; set; }
        public string DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteTime { get; set; }
        public string FileWord { get; set; }
        public string PostRequest { get; set; }
        public string JobResponsibility { get; set; }
        public Nullable<int> IsHaveBeen { get; set; }
        public string InterviewAddress { get; set; }
        public string Principal { get; set; }
        public int NeedsPostId { get; set; }
        public virtual TErp_Department TErp_Department { get; set; }
        public virtual TErp_Position TErp_Position { get; set; }
        public virtual ICollection<THR_Recruit> THR_Recruit { get; set; }
    }
}
