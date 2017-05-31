using System;
using System.Collections.Generic;

namespace HKSJRecruitment.Models.Models
{
    public partial class Tapp_ParamType
    {
        public int Id { get; set; }
        public string ParamName { get; set; }
        public string ParamIcon { get; set; }
        public int Sort { get; set; }
        public int ParentId { get; set; }
        public int IsShow { get; set; }
    }
}
