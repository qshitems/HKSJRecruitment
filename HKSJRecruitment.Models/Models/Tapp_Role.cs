using System;
using System.Collections.Generic;

namespace HKSJRecruitment.Models.Models
{
    public partial class Tapp_Role
    {
        public Tapp_Role()
        {
            this.Tapp_Role_Right = new List<Tapp_Role_Right>();
            this.Tapp_User_Role = new List<Tapp_User_Role>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public string Remarks { get; set; }
        public virtual ICollection<Tapp_Role_Right> Tapp_Role_Right { get; set; }
        public virtual ICollection<Tapp_User_Role> Tapp_User_Role { get; set; }
    }
}
