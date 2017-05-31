using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKSJRecruitment.Web.Models
{
    public class DepartmentDto
    {
        public DepartmentDto()
        {
        }
        public int Id { get; set; }
        public string DeptName { get; set; }
        public string DeptType { get; set; }
        public string DeptIcon { get; set; }
        public string DeptMaster { get; set; }
        public string DeptTel { get; set; }
        public string DeptFax { get; set; }
        public string DeptAddr { get; set; }
        public string DeptEmail { get; set; }
        public string DeptDesc { get; set; }
        public int SeqNo { get; set; }
        public int ParentId { get; set; }
    }
}