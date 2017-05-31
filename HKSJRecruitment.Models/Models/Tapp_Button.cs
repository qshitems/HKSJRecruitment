using System;
using System.Collections.Generic;

namespace HKSJRecruitment.Models.Models
{
    public partial class Tapp_Button
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public string ButtonText { get; set; }
        public string HandlerUrl { get; set; }
        public string ButtonJsId { get; set; }
        public string ButtonIcon { get; set; }
        public string ButtonClass { get; set; }
        public int SeqNo { get; set; }
        public int IsShow { get; set; }
        public string Remarks { get; set; }
        public virtual Tapp_Menu Tapp_Menu { get; set; }
    }
}
