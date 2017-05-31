using Common.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKSJRecruitment.Web.Models
{
    public class Hr_NeedQueryParam : QueryParam
    {
        public int DeptId { get; set; }
        public int PostId { get; set; }
        public int NeedQuantity { get; set; }
        public string Demander { get; set; }
        public Nullable<System.DateTime> CutTimeStart { get; set; }
        public Nullable<System.DateTime> CutTimeEnd { get; set; }
        /// <summary>
        /// 已招聘人数
        /// </summary>
        public int HaveBeenQuantity { get; set; }


        public int? IsHaveBeen { get; set; }

        //public int Type { get; set; }

        public Hr_NeedQueryParam()
        {
            IsHaveBeen = 0;
            //Type = 0;
        }

    }
}