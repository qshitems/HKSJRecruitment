using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKSJRecruitment.Web.Models
{
    public class UpdatePwdModel
    {
        public string OldPwd { get; set; }
        public string NewPwd { get; set; }
        public string ConfigPwd { get; set; }
    }
}