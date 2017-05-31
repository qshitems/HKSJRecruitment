using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using HKSJRecruitment.Models.Models;
using HKSJRecruitment.Web.Models;
using Common.Mvc;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;

namespace HKSJRecruitment.Web.Controllers
{
    public class RoleController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Buttons = this.LoadButton();
            return View();
        }

        public ActionResult List(RoleQueryParam queryParam)
        {
            Expression<Func<Tapp_Role, bool>> predicate = c => true;
            if (!string.IsNullOrEmpty(queryParam.RoleName))
            {
                predicate = predicate.And(c => c.RoleName.Contains(queryParam.RoleName));
            }
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            IPageList<Tapp_Role> models = ctx.Tapp_Role
                .Where(predicate)
                .OrderUsingSortExpression(queryParam.Sort, queryParam.Order)
                .ToPagedList(queryParam.PageIndex, queryParam.PageSize);
            List<Tapp_Role> dtos = models.ToList();
            return Content(this.GetJSON(new { total = models.PageInfo.TotalCount, rows = dtos }), this.JsonContentType());
        }
        public ActionResult Detail(int Id)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            Tapp_Role dto = ctx.Tapp_Role.FirstOrDefault(c => c.Id == Id);
            if (dto != null)
            {
                return Content(this.GetJSON(new { Result = true, Dto = dto }), this.JsonContentType());
            }
            else
            {
                return Content(this.GetJSON(new { Result = false, Msg = "未找到数据" }), this.JsonContentType());
            }
        }
        [HttpPost]
        public ActionResult Add(Tapp_Role dto)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            ctx.Tapp_Role.Add(dto);
            if (ctx.SaveChanges() >= 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功", Dto = dto }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败", Dto = dto }), this.JsonContentType());
        }

        [HttpPost]
        public ActionResult Edit(Tapp_Role dto)
        {
            if (dto.Id > 0)
            {
                HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
                ctx.Entry<Tapp_Role>(dto).State = System.Data.Entity.EntityState.Modified;
                if (ctx.SaveChanges() >= 0)
                {
                    return Content(this.GetJSON(new { Result = true, Msg = "成功", Dto = dto }), this.JsonContentType());
                }
                return Content(this.GetJSON(new { Result = false, Msg = "失败", Dto = dto }), this.JsonContentType());
            }
            else
            {
                return Content(this.GetJSON(new { Result = false, Msg = "失败，未找到要修改的数据", Dto = dto }), this.JsonContentType());
            }
        }

        [HttpPost]
        public ActionResult DeleteMulti(string Id)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            string[] arrId = Id.Split(new char[] { ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            int counter = 0;
            foreach (var item in arrId)
            {
                int tid = Convert.ToInt32(item);
                Tapp_Role model = ctx.Tapp_Role.FirstOrDefault(c => c.Id == tid);
                if (model != null)
                {
                    ctx.Tapp_Role.Remove(model);
                    counter++;
                }
            }
            if (ctx.SaveChanges() >= 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功", Id = counter }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败", id = counter }), this.JsonContentType());
        }
        public ActionResult RightEdit(int roleid, string butIds)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            string[] arrButIds = butIds.Split(new char[] { ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string[] arrDbIds = ctx.Tapp_Role_Right.Where(c => c.RoleId == roleid && c.ButtonId > 0).Select(c => c.ButtonId.ToString()).ToArray();
            foreach (var item in arrDbIds.Except(arrButIds))
            {
                //删除
                int buttonId = Convert.ToInt32(item);
                Tapp_Role_Right right = ctx.Tapp_Role_Right.FirstOrDefault(c => c.RoleId == roleid && c.ButtonId == buttonId);
                if (right != null)
                {
                    ctx.Tapp_Role_Right.Remove(right);
                }
            }
            foreach (var item in arrButIds.Except(arrDbIds))
            {
                //新增
                int buttonId = Convert.ToInt32(item);
                Tapp_Button button = ctx.Tapp_Button.FirstOrDefault(c => c.Id == buttonId);
                if (button != null)
                {
                    ctx.Tapp_Role_Right.Add(new Tapp_Role_Right { RoleId = roleid, MenuId = button.MenuId, ButtonId = button.Id });
                }
            }
            int ret = ctx.SaveChanges();
            //检查菜单权限
            List<int> menuIds = ctx.Tapp_Role_Right.Where(c => c.RoleId == roleid && c.ButtonId > 0).Select(c => c.MenuId).Distinct().ToList();
            List<int> menuIds2 = ctx.Tapp_Role_Right.Where(c => c.RoleId == roleid && c.ButtonId == 0).Select(c => c.MenuId).Distinct().ToList();
            foreach (var item in menuIds2.Except(menuIds))
            {
                //删除菜单权限
                Tapp_Role_Right right = ctx.Tapp_Role_Right.FirstOrDefault(c => c.RoleId == roleid && c.MenuId == item && c.ButtonId == 0);
                ctx.Tapp_Role_Right.Remove(right);
            }
            foreach (var item in menuIds.Except(menuIds2))
            {
                //新增菜单权限
                ctx.Tapp_Role_Right.Add(new Tapp_Role_Right { RoleId = roleid, MenuId = item, ButtonId = 0 });
            }
            int ret2 = ctx.SaveChanges();
            if (ret > 0 || ret2 > 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功" }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败" }), this.JsonContentType());
        }
        public ActionResult UserRight(string userids, string roleids)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            string[] arrRoleId = roleids.Split(new char[] { ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var username in userids.Split(new char[] { ',', '-' }, StringSplitOptions.RemoveEmptyEntries))
            {
                Tapp_User appUser = ctx.Tapp_User.FirstOrDefault(c => c.UserName == username);
                if (appUser == null)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "员工[" + username + "]不存在" }), this.JsonContentType());
                }
                string[] arrDbRoleId = ctx.Tapp_User_Role.Where(c => c.UserId == appUser.Id).Select(c => c.RoleId.ToString()).ToArray();
                foreach (var item in arrDbRoleId.Except(arrRoleId))
                {
                    //删除
                    int roleId = Convert.ToInt32(item);
                    Tapp_User_Role right = ctx.Tapp_User_Role.FirstOrDefault(c => c.RoleId == roleId && c.UserId == appUser.Id);
                    if (right != null)
                    {
                        ctx.Tapp_User_Role.Remove(right);
                    }
                }
                foreach (var item in arrRoleId.Except(arrDbRoleId))
                {
                    //新增
                    int roleId = Convert.ToInt32(item);
                    Tapp_Role role = ctx.Tapp_Role.FirstOrDefault(c => c.Id == roleId);
                    if (role != null)
                    {
                        ctx.Tapp_User_Role.Add(new Tapp_User_Role { RoleId = roleId, UserId = appUser.Id });
                    }
                }
            }
            if (ctx.SaveChanges() >= 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功" }), this.JsonContentType());
            }
            else
            {
                return Content(this.GetJSON(new { Result = false, Msg = "失败" }), this.JsonContentType());
            }
        }
        [Anonymous]
        public ActionResult MenuButton(int roleId)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            List<Tapp_Role_Right> roleRights = ctx.Tapp_Role_Right.Where(c => c.RoleId == roleId).ToList();
            List<Tapp_Menu> dtos = ctx.Tapp_Menu.ToList();

            List<MenuButtonDto> menus = new List<MenuButtonDto>();
            foreach (var item in dtos.Where(c => c.ParentId == 0).OrderBy(c => c.SeqNo))
            {
                MenuButtonDto menuItem = new MenuButtonDto();
                menuItem.Id = item.Id;
                menuItem.MenuText = item.MenuText;
                List<Tapp_Menu> menuSubs = dtos.Where(c => c.ParentId == item.Id).OrderBy(c => c.SeqNo).ToList();
                if (menuSubs.Count > 0)
                {
                    menuItem.Children = new List<MenuButtonDto>();
                    foreach (var sub in menuSubs)
                    {
                        MenuButtonDto menuSub = new MenuButtonDto();
                        menuSub.Id = sub.Id;
                        menuSub.MenuText = sub.MenuText;
                        string buttonText = string.Empty;
                        foreach (var button in ctx.Tapp_Button.Where(c => c.MenuId == sub.Id).OrderBy(c => c.SeqNo))
                        {
                            buttonText += button.Id + "," + button.ButtonText + "," + (roleRights.Exists(c => c.ButtonId == button.Id) ? "1" : "0") + ";";
                        }
                        menuSub.ButtonText = buttonText.Trim(';');
                        menuItem.Children.Add(menuSub);
                    }
                }
                menus.Add(menuItem);
            }
            return Content(this.GetJSON(menus), this.JsonContentType());

        }
    }
}
