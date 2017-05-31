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
    public class ButtonController : Controller
    {
        public ActionResult Index()
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            List<Tapp_Menu> Menus = ctx.Tapp_Menu.OrderUsingSortExpression("parentid asc,seqno asc").ToList();
            foreach (var item in Menus)
            {
                item.Tapp_Button.Clear();
                item.Tapp_Role_Right.Clear();
            }
            ViewBag.Buttons = this.LoadButton();
            ViewBag.Menus = this.GetJSON(Menus);
            return View();
        }

        public ActionResult List(ButtonQueryParam queryParam)
        {
            Expression<Func<Tapp_Button, bool>> predicate = c => true;
            if (queryParam.MenuId > 0)
            {
                predicate = predicate.And(c => c.MenuId == queryParam.MenuId);
            }
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            IPageList<Tapp_Button> models = ctx.Tapp_Button
                .Where(predicate)
                .OrderUsingSortExpression(queryParam.Sort, queryParam.Order)
                .ToPagedList(queryParam.PageIndex, queryParam.PageSize);
            List<Tapp_Button> dtos = models.ToList();
            return Content(this.GetJSON(new { total = models.PageInfo.TotalCount, rows = dtos }), this.JsonContentType());
        }
        public ActionResult Detail(int Id)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            Tapp_Button dto = ctx.Tapp_Button.FirstOrDefault(c => c.Id == Id);
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
        public ActionResult Add(Tapp_Button dto)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            ctx.Tapp_Button.Add(dto);
            if (ctx.SaveChanges() >= 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功", Dto = dto }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败", Dto = dto }), this.JsonContentType());
        }

        [HttpPost]
        public ActionResult Edit(Tapp_Button dto)
        {
            if (dto.Id > 0)
            {
                HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
                ctx.Entry<Tapp_Button>(dto).State = System.Data.Entity.EntityState.Modified;
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
                Tapp_Button model = ctx.Tapp_Button.FirstOrDefault(c => c.Id == tid);
                if (model != null)
                {
                    ctx.Tapp_Button.Remove(model);
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
        public ActionResult ListMenu()
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            List<Tapp_Menu> Menus = ctx.Tapp_Menu.OrderBy(c => c.SeqNo).ToList();
            return Content(this.GetJSON(new { Result = true, Msg = "成功", Dtos = Menus }), this.JsonContentType());
        }
    }
}
