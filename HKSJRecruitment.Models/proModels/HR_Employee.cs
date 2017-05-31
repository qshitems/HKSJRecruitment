using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKSJRecruitment.Models.proModels
{
   public class HR_Employee
    {
        public int ID { get; set; }
        public string uid { get; set; }
        public string pwd { get; set; }
        public string name { get; set; }
        public string idcard { get; set; }
        public string birthday { get; set; }
        public Nullable<int> dptid { get; set; }
        public string dptname { get; set; }
        public Nullable<int> postid { get; set; }
        public string post { get; set; }
      
        public string sex { get; set; }
        public string tel { get; set; }
        public string status { get; set; }
      
        public string EntryDate { get; set; }
        public string address { get; set; }
     
        public string education { get; set; }
        public string schools { get; set; }
      
     
    }
}
