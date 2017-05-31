using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKSJRecruitment.Web.Models
{
    public class MenuDto
    {
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
        public System.DateTime CreateTime { get; set; }
        public List<MenuDto> Children { get; set; }
        public string IsShowDesc
        {
            get
            {
                if (IsShow == 0) return "否";
                else return "是";
            }
        }
    }
    public class MenuButtonDto
    {
        public MenuButtonDto()
        {
            MenuText = string.Empty;
            ButtonText = string.Empty;
        }
        public int Id { get; set; }
        public string MenuText { get; set; }
        /// <summary>
        /// 格式：按钮Id,按钮文本，是否有权限[0-无，1-有];
        /// </summary>
        public string ButtonText{get;set;}
        public List<MenuButtonDto> Children { get; set; }
    }
}