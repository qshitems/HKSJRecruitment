using System;
using System.Collections.Generic;

namespace HKSJRecruitment.Models.Models
{
    public partial class Report_ZhiweistatisticsDay
    {
        public int Id { get; set; }
        public string Dptname { get; set; }
        public string Zhiweiname { get; set; }
        public Nullable<int> Querentime_couunt { get; set; }
        public Nullable<int> Xianchang_count { get; set; }
        public Nullable<int> Phone_count { get; set; }
        public Nullable<int> Luyong_count { get; set; }
        public Nullable<int> Taotai_count { get; set; }
        public Nullable<int> Beiyong_count { get; set; }
        public Nullable<int> Daihuifu_count { get; set; }
        public Nullable<int> Phonetongzhi_count { get; set; }
        public Nullable<int> Mailtongzhi_count { get; set; }
        public Nullable<System.DateTime> Createon { get; set; }
    }
}
