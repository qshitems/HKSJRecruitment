using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HKSJRecruitment.Models.Models;
using System.Web;
using Common.Mvc;
using System.Web.Mvc;
using HKSJRecruitment.Web.Models;

namespace HKSJRecruitment.Web
{
    /// <summary>
    /// 权限认证
    /// </summary>
    public class RightHeper
    {
        /// <summary>
        /// 判断用户否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool HasUser(int userId)
        {
            HKSJRecruitmentContext ctx = HttpContext.Current.GetDbContext<HKSJRecruitmentContext>();
            return ctx.Tapp_User.Count(c => c.Id == userId) > 0;
        }
        /// <summary>
        /// 加载用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<string> LoadUserRight(int userId)
        {
            HKSJRecruitmentContext ctx = HttpContext.Current.GetDbContext<HKSJRecruitmentContext>();
            return ctx.Vapp_UserRight.Where(c => c.UserId == userId).Select(c => c.Url).ToList();
        }
        /// <summary>
        /// 获取权限菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Tapp_Menu> LoadMenu(int userId)
        {
            HKSJRecruitmentContext ctx = HttpContext.Current.GetDbContext<HKSJRecruitmentContext>();
            string sql = string.Empty;
            if (UserHelper.IgnoreCheckRight)
            {
                return ctx.Tapp_Menu.Where(c => c.IsShow == 1).ToList();
            }
            else
            {
                sql = string.Format(@"
with cte(Id,MenuCode,MenuText,MenuUrl,MenuIcon,MenuClass,SeqNo,ParentId,IsShow,Remarks)
as
(--下级父项
select distinct menu.Id,menu.MenuCode,menu.MenuText,menu.MenuUrl,menu.MenuIcon,menu.MenuClass,menu.SeqNo,menu.ParentId,menu.IsShow,menu.Remarks  
from Tapp_User_Role roles
inner join Tapp_Role_Right rights on roles.RoleId = rights.RoleId
inner join Tapp_Menu menu on  menu.Id = rights.MenuId where roles.UserId = {0} and menu.IsShow = 1
union all
--递归结果集中的父项
select t.Id,t.MenuCode,t.MenuText,t.MenuUrl,t.MenuIcon,t.MenuClass,t.SeqNo,t.ParentId,t.IsShow,t.Remarks from Tapp_Menu as t
inner join cte as c on t.id = c.parentid
)
select distinct Id,MenuCode,MenuText,MenuUrl,MenuIcon,MenuClass,SeqNo,ParentId,IsShow,Remarks from cte
order by ParentId asc,SeqNo asc", userId);
                return ctx.Database.SqlQuery<Tapp_Menu>(sql).ToList();
            }
        }
        /// <summary>
        /// 获取权限按钮
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public List<Tapp_Button> LoadButton(int userId, int menuId)
        {
            HKSJRecruitmentContext ctx = HttpContext.Current.GetDbContext<HKSJRecruitmentContext>();
            if (UserHelper.IgnoreCheckRight)
            {
                return ctx.Tapp_Button.Where(c => c.MenuId == menuId && c.IsShow == 1).OrderBy(c => c.SeqNo).ToList();
            }
            else
            {
                var query = from role in ctx.Tapp_Role_Right
                            join button in ctx.Tapp_Button on role.ButtonId equals button.Id
                            join user in ctx.Tapp_User_Role on role.RoleId equals user.RoleId
                            where user.UserId == userId && role.MenuId == menuId && button.IsShow == 1
                            orderby button.SeqNo ascending
                            select button;
                return query.Distinct().ToList();
            }
        }
        /// <summary>
        /// 获取权限按钮
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public List<Tapp_Button> LoadButton(int userId, string controller, string action)
        {
            string url = "/" + controller + "/" + action;
            HKSJRecruitmentContext ctx = HttpContext.Current.GetDbContext<HKSJRecruitmentContext>();
            Tapp_Menu menu = ctx.Tapp_Menu.FirstOrDefault(c => c.MenuUrl == url);
            if (menu != null)
            {
                return LoadButton(userId, menu.Id);
            }
            return null;
        }
        /// <summary>
        /// 获取权限按钮
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="menuCode"></param>
        /// <returns></returns>
        public List<Tapp_Button> LoadButton(int userId, string menuCode)
        {
            HKSJRecruitmentContext ctx = HttpContext.Current.GetDbContext<HKSJRecruitmentContext>();
            Tapp_Menu menu = ctx.Tapp_Menu.FirstOrDefault(c => c.MenuCode == menuCode);
            if (menu != null)
            {
                return LoadButton(userId, menu.Id);
            }
            return null;
        }
        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public List<UserRoleDto> LoadUserRole(int userId)
        {
            HKSJRecruitmentContext ctx = HttpContext.Current.GetDbContext<HKSJRecruitmentContext>();
            return ctx.Tapp_User_Role.Where(c => c.UserId == userId).Select(c => new UserRoleDto
            {
                UserId = c.UserId,
                IsRight = true,
                RoleId = c.RoleId,
                RoleName = c.Tapp_Role.RoleName
            }).ToList();
        }
        /// <summary>
        /// 获取当前登录用户角色
        /// </summary>
        /// <returns></returns>
        public List<UserRoleDto> LoadUserRole()
        {
            return LoadUserRole(HttpContext.Current.AppUserId());
        }
    }
}
