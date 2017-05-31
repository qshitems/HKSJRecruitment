using System;
using System.Collections.Generic;

namespace HKSJRecruitment.Models.Models
{
    public partial class Tapp_Menu
    {
        public Tapp_Menu()
        {
            this.Tapp_Button = new List<Tapp_Button>();
            this.Tapp_Role_Right = new List<Tapp_Role_Right>();
        }

        public int Id { get; set; }
        public string MenuCode { get; set; }
        public string MenuText { get; set; }
        public string MenuUrl { get; set; }
        public string MenuIcon { get; set; }
        public string MenuClass { get; set; }
        public int SeqNo { get; set; }
        public int ParentId { get; set; }
        public int IsShow { get; set; }
        public string Remarks { get; set; }
        public virtual ICollection<Tapp_Button> Tapp_Button { get; set; }
        public virtual ICollection<Tapp_Role_Right> Tapp_Role_Right { get; set; }
    }
}
