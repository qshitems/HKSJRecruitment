using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKSJRecruitment.Web.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public string VerifyCode { get; set; }
    }
}