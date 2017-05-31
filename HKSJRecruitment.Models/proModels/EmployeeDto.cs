using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKSJRecruitment.Models.proModels
{
    public class EmployeeDto
    {

        public int Id { get; set; }
        public int AppUserId { get; set; }
        public string Uid { get; set; }
        public string Pwd { get; set; }
        public string DoaminAccount { get; set; }
        public string FullName { get; set; }
        public string Sex { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string IdCard { get; set; }
        public string Mobile { get; set; }
        public string Qq { get; set; }
        public string WeiXin { get; set; }
        public string Education { get; set; }
        public string School { get; set; }
        public string Major { get; set; }
        public string SocialCode { get; set; }
        public string HousenFund { get; set; }
        public decimal Salary { get; set; }
        public decimal SalaryTemporary { get; set; }
        public string HouseholdRegister { get; set; }
        public string Address { get; set; }
        public int DeptId { get; set; }
        public int PostId { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int EditBy { get; set; }
        public Nullable<System.DateTime> EditTime { get; set; }
        public int IsDelete { get; set; }
        public int DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteTime { get; set; }
        public string Email { get; set; }



    }
}
