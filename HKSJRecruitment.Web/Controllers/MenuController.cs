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
    public class MenuController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Buttons = this.LoadButton();
            return View();
        }
        public ActionResult List(QueryParam queryParam)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            List<MenuDto> dtos = ctx.Tapp_Menu.Select(c => new MenuDto
            {
                Id = c.Id,
                IsShow = c.IsShow,
                MenuClass = c.MenuClass,
                MenuCode = c.MenuCode,
                MenuIcon = c.MenuIcon,
                MenuText = c.MenuText,
                MenuUrl = c.MenuUrl,
                ParentId = c.ParentId,
                Remarks = c.Remarks,
                SeqNo = c.SeqNo
            }).ToList();
            List<MenuDto> menus = new List<MenuDto>();
            foreach (var item in dtos.Where(c => c.ParentId == 0).OrderBy(c => c.SeqNo))
            {
                MenuDto menuItem = item;
                List<MenuDto> subs = dtos.Where(c => c.ParentId == item.Id).ToList();
                if (subs.Count > 0)
                {
                    menuItem.Children = new List<MenuDto>();
                    foreach (var sub in subs)
                    {
                        menuItem.Children.Add(sub);
                    }
                }
                menus.Add(menuItem);
            }
            return Content(this.GetJSON(menus), this.JsonContentType());
        }
        public ActionResult Detail(int Id)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            Tapp_Menu dto = ctx.Tapp_Menu.FirstOrDefault(c => c.Id == Id);
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
        public ActionResult Add(Tapp_Menu dto)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            ctx.Tapp_Menu.Add(dto);
            if (ctx.SaveChanges() >= 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功", Dto = dto }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败", Dto = dto }), this.JsonContentType());
        }

        [HttpPost]
        public ActionResult Edit(Tapp_Menu dto)
        {
            if (dto.Id > 0)
            {
                HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
                ctx.Entry<Tapp_Menu>(dto).State = System.Data.Entity.EntityState.Modified;
                //Tapp_Menu model = ctx.Tapp_Menu.Find(dto.Id);
                //model.MenuText = dto.MenuText;
                //model.MenuCode = dto.MenuCode;
                //model.MenuClass = dto.MenuClass;
                //model.MenuIcon = dto.MenuIcon;
                //model.MenuUrl = dto.MenuUrl;
                //model.SeqNo = dto.SeqNo;
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
                Tapp_Menu model = ctx.Tapp_Menu.Include(c => c.Tapp_Button).Include(c => c.Tapp_Role_Right).FirstOrDefault(c => c.Id == tid);
                if (model != null)
                {
                    int parentId = model.Id;
                    if (model.Tapp_Button.Count > 0 || model.Tapp_Role_Right.Count > 0 || ctx.Tapp_Menu.Count(c => c.ParentId == parentId) > 0)
                    {
                        return Content(this.GetJSON(new { Result = false, Msg = "此菜单已被引用，不能删除", id = counter }), this.JsonContentType());
                    }
                    ctx.Tapp_Menu.Remove(model);
                    counter++;
                }
            }
            if (ctx.SaveChanges() >= 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功", Id = counter }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败", id = counter }), this.JsonContentType());
        }
        [Anonymous]
        public ActionResult ListMenuParent()
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            return Content(this.GetJSON(ctx.Tapp_Menu.Where(c => c.ParentId == 0).OrderBy(c => c.SeqNo).Select(c => new { Id = c.Id, Text = c.MenuText }).ToList()));
        }
    }
}
