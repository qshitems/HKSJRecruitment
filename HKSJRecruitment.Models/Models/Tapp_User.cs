using System;
using System.Collections.Generic;

namespace HKSJRecruitment.Models.Models
{
    public partial class Tapp_User
    {
        public Tapp_User()
        {
            this.Tapp_User_Role = new List<Tapp_User_Role>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public int State { get; set; }
        public virtual ICollection<Tapp_User_Role> Tapp_User_Role { get; set; }
    }
}
