using System;
using System.Collections.Generic;

namespace HKSJRecruitment.Models.Models
{
    public partial class Tapp_User_Role
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public virtual Tapp_Role Tapp_Role { get; set; }
        public virtual Tapp_User Tapp_User { get; set; }
    }
}
