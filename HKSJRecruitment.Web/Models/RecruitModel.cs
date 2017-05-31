
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKSJRecruitment.Web.Models
{
    public class RecruitModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DptName { get; set; }
        /// <summary>
        /// 职位名称
        /// </summary>
        public string PostName { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string  StatusName { get; set; }
        /// <summary>
        /// 面试时间
        /// </summary>
        public Nullable<System.DateTime> Interview { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public Nullable<System.DateTime> CreateTime { get; set; }
        /// <summary>
        /// 添加人
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 简历地址
        /// </summary>
        public string Userurl { get; set; }
        /// <summary>
        /// 报道时间
        /// </summary>
        public Nullable<System.DateTime> HireTime { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 招聘需求名称
        /// </summary>
        public string NeedsName { get; set; }
        /// <summary>
        /// 招聘需求ID
        /// </summary>
        public int? NeedsId { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public int? DptId { get; set; }
        /// <summary>
        /// 职位ID
        /// </summary>
        public int? PostId { get; set; }
        /// <summary>
        /// 状态ID
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 入职状态ID（1待入职，2未入职，3已入职）
        /// </summary>
        public int? EntryType {get; set;}
        /// <summary>
        /// 格式化处理报道时间
        /// </summary>
        public string StrHireTime 
        { 
            get
            {
                if(HireTime != null)
                {
                    return HireTime.Value.ToString("yyyy-MM-dd");
                }
                return "";
            }
            
        }
        /// <summary>
        /// 格式化处理面试时间
        /// </summary>
        public string StrInterview
        {
            get
            {
                if (Interview != null)
                {
                    return Interview.Value.ToString("yyyy-MM-dd");
                }
                return "";
            }
        }
        /// <summary>
        /// 删除状态 0 未删除 1 已删除
        /// </summary>
        public int IsDelete { get; set; }
        /// <summary>
        /// 编辑人
        /// </summary>
        public string EditBy { get; set; }
        /// <summary>
        /// 编辑时间
        /// </summary>
        public Nullable<System.DateTime> EditTime { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>
        public string DeleteBy { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public Nullable<System.DateTime> DeleteTime { get; set; }
        /// <summary>
        /// 入职状态
        /// </summary>
        public string EntryName
        {
            get
            {
                if (EntryType.HasValue)
                {
                    int tp = EntryType.Value;
                    if(tp==1)
                    {
                        return "待入职";
                    }
                    if(tp==2)
                    {
                        return "未入职";
                    }
                    if(tp==3)
                    {
                        return "已入职";
                    }

                }
                return "";
            }
        }
        /// <summary>
        /// 面试官
        /// </summary>
        public string Interviewer { get; set; }

        public Nullable<int> IsHaveBeen { get; set; }
    
    }
}