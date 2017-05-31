using System;
using System.Collections.Generic;

namespace HKSJRecruitment.Models.Models
{
    public partial class HR_Employee
    {
        public int ID { get; set; }
        public string uid { get; set; }
        public string pwd { get; set; }
        public string name { get; set; }
        public string idcard { get; set; }
        public string birthday { get; set; }
        public Nullable<int> dptid { get; set; }
        public string dptname { get; set; }
        public Nullable<int> postid { get; set; }
        public string post { get; set; }
        public string email { get; set; }
        public string sex { get; set; }
        public string tel { get; set; }
        public string status { get; set; }
        public Nullable<int> zhiwuid { get; set; }
        public string zhiwu { get; set; }
        public Nullable<int> sort { get; set; }
        public string EntryDate { get; set; }
        public string address { get; set; }
        public string remarks { get; set; }
        public string education { get; set; }
        public string level { get; set; }
        public string professional { get; set; }
        public string schools { get; set; }
        public string title { get; set; }
        public Nullable<int> isDelete { get; set; }
        public Nullable<System.DateTime> Delete_time { get; set; }
        public string portal { get; set; }
        public string theme { get; set; }
        public Nullable<int> canlogin { get; set; }
    }
}
