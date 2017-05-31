using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKSJRecruitment.Web.Models
{
    public class PostDto
    {
        public int Id { get; set; }
        public string PostName { get; set; }
        public string PostDesc { get; set; }
        public int PositionId { get; set; }
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public string PositionName { get; set; }
        public string PositionLevel { get; set; }

    }
}