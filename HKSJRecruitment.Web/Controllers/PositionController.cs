using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Mvc;
using System.Data.Entity;
using HKSJRecruitment.Models.Models;
using System.Linq.Expressions;

namespace HKSJRecruitment.Web.Controllers
{
    public class PositionController : Controller
    {
        //
        // GET: /Position/

        public ActionResult Index()
        {
            ViewBag.Buttons = this.LoadButton();
            return View();
        }

        public ActionResult List(QueryParam queryParam)
        {
            Expression<Func<TErp_Position, bool>> predicate = c => c.IsDelete == 0;
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            IPageList<TErp_Position> models = ctx.TErp_Position
                .Where(predicate)
                .OrderUsingSortExpression(queryParam.Sort, queryParam.Order)
                .ToPagedList(queryParam.PageIndex, queryParam.PageSize);
            List<TErp_Position> dtos = models.ToList();
            return Content(this.GetJSON(new { total = models.PageInfo.TotalCount, rows = dtos }), this.JsonContentType());
        }
        public ActionResult Detail(int Id)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            TErp_Position dto = ctx.TErp_Position.FirstOrDefault(c => c.Id == Id);
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
        public ActionResult Add(TErp_Position dto)
        {
            HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
            string positionName = dto.PositionName;
            if (ctx.TErp_Position.Count(c => c.PositionName == positionName && c.IsDelete == 0) > 0)
            {
                return Content(this.GetJSON(new { Result = false, Msg = "职位名称[" + positionName + "]重复" }), this.JsonContentType());
            }
            dto.CreateBy = this.AppUser().UserName;
            dto.CreateTime = DateTime.Now;
            ctx.TErp_Position.Add(dto);
            if (ctx.SaveChanges() > 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功", Dto = dto }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败", Dto = dto }), this.JsonContentType());
        }

        [HttpPost]
        public ActionResult Edit(TErp_Position dto)
        {
            if (dto.Id > 0)
            {
                HKSJRecruitmentContext ctx = HttpContext.GetDbContext<HKSJRecruitmentContext>();
                string positionName = dto.PositionName;
                int id = dto.Id;
                if (ctx.TErp_Position.Count(c => c.PositionName == positionName && c.Id != id && c.IsDelete == 0) > 0)
                {
                    return Content(this.GetJSON(new { Result = false, Msg = "职位名称[" + positionName + "]重复" }), this.JsonContentType());
                }
                TErp_Position model = ctx.TErp_Position.FirstOrDefault(c => c.Id == dto.Id);
                this.CopyObject<TErp_Position>(model, dto);
                Tapp_User user = this.AppUser();
                model.EditBy = user.UserName;
                model.EditTime = DateTime.Now;
                if (ctx.SaveChanges() > 0)
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
            Tapp_User user = this.AppUser();
            foreach (var item in arrId)
            {
                int tid = Convert.ToInt32(item);
                TErp_Position model = ctx.TErp_Position.FirstOrDefault(c => c.Id == tid);
                if (model != null)
                {
                    int postId = model.Id;
                    if (ctx.THR_Needs.Count(c => c.PostId == postId) > 0
                        || ctx.THR_Recruit.Count(c => c.PostId == postId) > 0)
                    {
                        return Content(this.GetJSON(new { Result = false, Msg = "职位[" + model.PositionName + "]已被引用,不能删除", Id = counter }), this.JsonContentType());
                    }
                    model.IsDelete = 1;
                    model.DeleteBy = user.UserName;
                    model.DeleteTime = DateTime.Now;
                    counter++;
                }
            }
            if (ctx.SaveChanges() > 0)
            {
                return Content(this.GetJSON(new { Result = true, Msg = "成功", Id = counter }), this.JsonContentType());
            }
            return Content(this.GetJSON(new { Result = false, Msg = "失败", id = counter }), this.JsonContentType());
        }
    }
}
