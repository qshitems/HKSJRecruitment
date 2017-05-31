using Common.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKSJRecruitment.Web.Models
{
    public class RoleQueryParam : QueryParam
    {
        public string RoleName { get; set; }
    }
}