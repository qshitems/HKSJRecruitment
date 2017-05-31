using System;
using System.Collections.Generic;

namespace HKSJRecruitment.Models.Models
{
    public partial class Tapp_Role_Right
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public int ButtonId { get; set; }
        public virtual Tapp_Menu Tapp_Menu { get; set; }
        public virtual Tapp_Role Tapp_Role { get; set; }
    }
}
