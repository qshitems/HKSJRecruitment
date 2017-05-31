using Common.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKSJRecruitment.Web.Models
{
    public class RecruitQueryParam : QueryParam
    {

        public string Name { get; set; }
        public string Tel { get; set; }
        public string interviewer { get; set; }
        public int DptId { get; set; }
        public int PostId { get; set; }
        public int Status { get; set; }
        public int QueryType { get; set; }

        public Nullable<System.DateTime> Interview { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> IntervieweStart { get; set; }
        public Nullable<System.DateTime> IntervieweEnd { get; set; }

        public Nullable<System.DateTime> CreateTimeStart { get; set; }
        public Nullable<System.DateTime> CreateTimeEnd { get; set; }
        

        public string CreateBy { get; set; }
        public int IsDelete { get; set; }
    }
}