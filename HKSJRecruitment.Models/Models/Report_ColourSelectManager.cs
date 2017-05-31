using System;
using System.Collections.Generic;

namespace HKSJRecruitment.Models.Models
{
    public partial class Report_ColourSelectManager
    {
        public int Id { get; set; }
        public string Manager { get; set; }
        public string Colour { get; set; }
        public Nullable<int> Sort { get; set; }
    }
}
