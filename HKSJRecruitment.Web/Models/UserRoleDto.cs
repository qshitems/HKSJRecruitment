using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKSJRecruitment.Web.Models
{
    public class UserRoleDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsRight { get; set; }
    }
}