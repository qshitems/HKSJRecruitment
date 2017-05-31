using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKSJRecruitment.Web.Models
{
    public class Hr_NeedDto
    {
        public int Id { get; set; }
        public int DeptId { get; set; }
        public int PostId { get; set; }
        public int NeedQuantity { get; set; }
        public string Demander { get; set; }
        public Nullable<System.DateTime> CutTime { get; set; }
        public string Remarks { get; set; }
        public int IsDelete { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditTime { get; set; }
        public string DeleteBy { get; set; }
        public Nullable<System.DateTime> DeleteTime { get; set; }
        public string DeptName { get; set; }
        public string PostName { get; set; }
        /// <summary>
        /// 已招聘人数
        /// </summary>
        public int HaveBeenQuantity { get; set; }
        /// <summary>
        /// 试题路径
        /// </summary>
        public string FileWord { get; set; }
        /// <summary>
        /// 是否有试题
        /// </summary>
        public string IsFileWord
        {
            get
            {
                if (!string.IsNullOrEmpty(FileWord))
                { return "是"; }
                return "否";
            }
        }
        /// <summary>
        /// 岗位要求
        /// </summary>
        public string PostRequest { get; set; }
        /// <summary>
        /// 工作职责
        /// </summary>
        public string JobResponsibility { get; set; }
        /// <summary>
        /// 是否招聘满 0 否 1 是
        /// </summary>
        public int IsHaveBeen { get; set; }

        public string StrCuttime
        {
            get
            {
                if (CutTime != null)
                {
                    return CutTime.Value.ToString("yyyy-MM-dd");
                }
                return "";
            }
        }
        public string StrIsHaveBeen
        {
            get
            {
                if (IsHaveBeen == 1)
                {
                    return "已完成";
                }
                return "未完成";
            }
        }
        public string StrDeptPost
        {
            get { return DeptName + " " + PostName; }
        }
        /// <summary>
        /// 面试地址
        /// </summary>
        public string InterviewAddress
        { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string Principal { get; set; }

        public int NeedsPostId { get; set; }
    }
}