using Common.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKSJRecruitment.Web.Models
{
    public class EmployeeQueryParam : QueryParam
    {
        public string name { get; set; }
        public int deptid { get; set; }
    }
}
